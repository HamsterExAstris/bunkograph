import dayjs from 'dayjs'
import Highcharts, { PointMarkerOptionsObject, PointOptionsObject } from 'highcharts';
import highchartsAccessibility from "highcharts/modules/accessibility";
import HighchartsReact from 'highcharts-react-official';
import { useEffect, useState } from 'react';
import { ISeriesInfo } from './Series';

// Initialize Highcharts for accessibility.
highchartsAccessibility(Highcharts);

export interface IGraphSampleProps {
  seriesInfo: ISeriesInfo
}

interface IPublishRate {
  averageEN?: string;
  averageJP?: string;
  averageAB?: string;
  weightedEN?: string;
  weightedJP?: string;
  weightedAB?: string;
}

interface ILocalizationTime {
  average: string;
  weighted: string;
  minimum: string;
  maximum: string;
}

interface IVolumeInfo {
  releaseDate: string; // Comes as a string from the API, need to deserialize into a Date ourselves.
  volumeNumber: number;
  label: string;
  language: string;
}

interface IPoint extends PointOptionsObject {
  label: string,
  marker?: PointMarkerOptionsObject
}

const populateVolumeData = async (seriesId: number) => {
  const response = await fetch('api/volumes/series/' + seriesId);
  const data = await response.json();

  return data as IVolumeInfo[];
}

const mapToPoint = function (v: IVolumeInfo): IPoint {
  var result: IPoint = {
    x: dayjs(v.releaseDate).unix() * 1000, // Highcharts uses milliseconds since the Unix epoch.
    y: v.volumeNumber,
    z: v.volumeNumber,
    label: v.label
  };

  if (v.label === "Next?") {
    result.marker = {
      fillColor: "#6c757d" // $secondary
    };
  }

  return result;
}

const buildOptions = function (series: string, publisher?: string, dataJP?: IPoint[], dataEN?: IPoint[], dataAB?: IPoint[]): Highcharts.Options {
  return {
    title: {
      text: '<h3>' + series + (publisher ? (' <small class="text-muted">' + publisher + '</small>') : "") + '</h3>',
      useHTML: true
    },
    xAxis: {
      type: 'datetime',
      dateTimeLabelFormats: {
        month: '%b %y',
        year: '%Y'
      },
      title: {
        text: 'Publication'
      },
      plotLines: [{
        color: '#dc3545', // $red
        width: 2,
        value: dayjs().unix() * 1000
      }]
    },
    yAxis: {
      title: { text: 'Volume' },
      min: 1,
      allowDecimals: false
    },
    tooltip: {
      headerFormat: 'Volume <b>{point.y}</b> ({series.name})<br/>',
      pointFormat: 'Publish date {point.x:%e/%b/%Y}'
    },
    plotOptions: {
      spline: {
        marker: {
          enabled: true
        }
      }
    },
    series: [
      {
        type: "spline",
        name: "JP",
        color: "#0d6efd", // $blue
        data: dataJP,
        dataLabels: {
          enabled: true,
          format: "{point.label}",
        }
      },
      {
        type: "spline",
        name: "EN",
        color: "#198754", // $green
        data: dataEN,
        dataLabels: {
          enabled: true,
          format: "{point.label}",
        }
      },
      {
        type: "spline",
        name: "AB",
        color: "#6f42c1", // $purple
        data: dataAB,
        dataLabels: {
          enabled: true,
          format: "{point.label}",
        }
      }
    ]
  }
}

const sumArrayValues = function(values: number[]) { return values.reduce(function (p, c) { return p + c }, 0); }

function mean(values: number[]) {
  return sumArrayValues(values) / values.length;
}

function indexWeightedMean(values: number[]) {
  const weight = function (pos: number, len: number) { return (pos + 1) / (len - pos); }
  const weights = values.map(function (x, i) { return weight(i, values.length) });
  return sumArrayValues(values.map(function (factor, index) { return factor * weights[index] })) / sumArrayValues(weights);
}

function lowestArrayValue(values: number[]) {
  return values.reduce((p, c) => (p < c ? p : c), Infinity);
}
function highestArrayValue(values: number[]) {
  return values.reduce((p, c) => (p > c ? p : c), -Infinity);
}

function nextOcurrenceInListWeighted(momentArray: dayjs.Dayjs[]): dayjs.Dayjs | undefined {
  var l = momentArray.length;
  if (l < 2) {
    return undefined;
  }

  var items = momentArray.map(function (x) { return x.unix(); }),
    dists = items.slice(1).map((v, i) => v - items[i]),
    avg = indexWeightedMean(dists)
    ;
  const res = dayjs(momentArray[l - 1]).add(avg, "seconds");
  return res;
}

const GraphSample: React.FC<IGraphSampleProps> = (props) => {
  const [options, setOptions] = useState<Highcharts.Options>(buildOptions(props.seriesInfo.englishName, props.seriesInfo.publisher));
  const [publishRate, setPublishRate] = useState<IPublishRate>();
  const [localizationTime, setLocalizationTime] = useState<ILocalizationTime>();

  useEffect(() => {
    const getAnswer = async () => {
      const volumes = await populateVolumeData(props.seriesInfo.seriesId);
      const jp: IVolumeInfo[] = [], en: IVolumeInfo[] = [], ab: IVolumeInfo[] = [];

      var leadtimes: number[] = [];
      const jpdate: dayjs.Dayjs[] = [], endate: dayjs.Dayjs[] = [], abdate: dayjs.Dayjs[] = [];

      for (const volume of volumes) {
        if (volume.language === "jp") {
          jp.push(volume);
          jpdate.push(dayjs(volume.releaseDate));
        }
        else if (volume.language === "en") {
          en.push(volume);
          endate.push(dayjs(volume.releaseDate));
        }
        else if (volume.language === "ab") {
          ab.push(volume);
          abdate.push(dayjs(volume.releaseDate));
        }
      }

      for (let i = 0; i < en.length; i++) {
        const releasejp = jp[i];
        if (releasejp)
        {
          const releasedatejp = dayjs(releasejp.releaseDate);
          const releaseen = dayjs(en[i].releaseDate);
          if (releaseen && releasedatejp) {
            leadtimes.push(releaseen.diff(releasedatejp, 'seconds'));
          }
        }
      }

      const now = dayjs();
      const isFinished = props.seriesInfo.completionStatus && props.seriesInfo.completionStatus !== "None" && props.seriesInfo.completionStatus !== "OneShot";
      const hasNoSource = false; // this.isHasNoSource();
      let nextjp: dayjs.Dayjs | undefined;
      let nexten: dayjs.Dayjs | undefined;
      let nextab: dayjs.Dayjs | undefined;
      let nexten_uncorrected: dayjs.Dayjs | undefined;
      let nextab_uncorrected: dayjs.Dayjs | undefined;
      let maxjp: number;
      let maxen: number;
      let maxab: number;

      maxjp = jp.reduce((a, v) => (a > v.volumeNumber ? a : v.volumeNumber), 0);
      //Can try to predict source if it isn't finished
      if (!isFinished) {
        nextjp = dayjs(nextOcurrenceInListWeighted(jpdate));
      }

      maxen = en.reduce((a, v) => (a > v.volumeNumber ? a : v.volumeNumber), 0);
      maxab = ab.reduce((a, v) => (a > v.volumeNumber ? a : v.volumeNumber), 0);
      // Can try to predict EN if:
      if (
        (!hasNoSource && //When it does have a source
          (
            maxen < maxjp //The release isn't caught up to source
            || nextjp //Or there is a prediction for next source release
          )
        ) || (hasNoSource && //Or when it doesn't have a source
          (
            !isFinished //If it isn't finished
          )
        )
      ) {
        nexten = nextOcurrenceInListWeighted(endate);
        nextab = nextOcurrenceInListWeighted(abdate);
      }

      nexten_uncorrected = nexten ? dayjs(nexten) : undefined;
      nextab_uncorrected = nextab ? dayjs(nextab) : undefined;

      if (nextjp && nextjp.isBefore(now)) { nextjp = now; }
      if (nexten && nexten.isBefore(now)) { nexten = now; }
      if (nextab && nextab.isBefore(now)) { nextab = now; }

      //Don't allow predict if predicted date doesn't account for weighted mean loc time (max 6 months)
      if (nexten && leadtimes.length) {
        const targetjp = maxen + 1;
        let _item : dayjs.Dayjs | undefined;
        if (nextjp && maxjp === maxen) {
          _item = dayjs(nextjp);
        } else {
          const item = jp.filter(x => x.volumeNumber === targetjp)[0];
          if (item) {
            _item = dayjs(item.releaseDate);
          }
        }
        if (_item) {
          // Minimum time a volume has taken to be released in English
          let lead = leadtimes.reduce((a, v) => (a < v ? a : v), Infinity);

          // Cap it at 10 months
          const maxlead = 60 * 60 * 24 * (365 / 12) * 10; // 10 months
          if (lead > maxlead) {
            lead = maxlead;
          }

          // Take into account this lead time when computing next release date
          const nexten_withlead = _item.add(lead, "seconds");
          if (nexten.isBefore(nexten_withlead)) {
            nexten = dayjs(nexten_withlead);
          }
          if (nexten_uncorrected?.isBefore(nexten_withlead)) {
            nexten_uncorrected = dayjs(nexten_withlead);
          }
        }
      }

      if (nextab && leadtimes.length) {
        const targetjp = maxen + 1;
        let _item: dayjs.Dayjs | undefined;
        if (nextjp && maxjp === maxab) {
          _item = dayjs(nextjp);
        } else {
          const item = jp.filter(x => x.volumeNumber === targetjp)[0];
          if (item) {
            _item = dayjs(item.releaseDate);
          }
        }
        if (_item) {
          // Minimum time a volume has taken to be released in English
          let lead = leadtimes.reduce((a, v) => (a < v ? a : v), Infinity);

          // Cap it at 10 months
          const maxlead = 60 * 60 * 24 * (365 / 12) * 10; // 10 months
          if (lead > maxlead) {
            lead = maxlead;
          }

          // Take into account this lead time when computing next release date
          const nextab_withlead = _item.add(lead, "seconds");
          if (nextab.isBefore(nextab_withlead)) {
            nextab = dayjs(nextab_withlead);
          }
          if (nextab_uncorrected?.isBefore(nextab_withlead)) {
            nextab_uncorrected = dayjs(nextab_withlead);
          }
        }
      }

      if (nextjp) {
        jp.push(
          {
            releaseDate: nextjp.format("YYYY-MM-DD"),
            volumeNumber: maxjp + 1,
            label: "Next?",
            language: "jp"
          }
        );
      }

      if (nexten) {
        en.push(
          {
            releaseDate: nexten.format("YYYY-MM-DD"),
            volumeNumber: maxen + 1,
            label: "Next?",
            language: "en"
          }
        );
      }

      if (nextab) {
        ab.push(
          {
            releaseDate: nextab.format("YYYY-MM-DD"),
            volumeNumber: maxab + 1,
            label: "Next?",
            language: "ab"
          }
        );
      }

      const dataJP: IPoint[] = jp.map(mapToPoint);
      const dataEN: IPoint[] = en.map(mapToPoint);
      const dataAB: IPoint[] = ab.map(mapToPoint);

      setOptions(buildOptions(props.seriesInfo.englishName, props.seriesInfo.publisher, dataJP, dataEN, dataAB));

      if (en.length > 1 || jp.length > 1) {
        const pubData: IPublishRate = {
        }

        if (en.length > 1) {
          let items = en.map(function (x) { return dayjs(x.releaseDate).unix() as number; }),
            dists = items.slice(1).map((v, i) => v - items[i]),
            sum = dists.reduce(function (a, b) { return a + b; })
          let avg = (sum / dists.length) / (30 * 24 * 60 * 60);
          let unit = "months";
          if (avg > 12) {
            avg /= 12;
            unit = "years";
          }
          avg = Math.floor(avg * 10) / 10;

          pubData.averageEN = avg + " " + unit;
        }

        if (ab.length > 1) {
          let items = ab.map(function (x) { return dayjs(x.releaseDate).unix() as number; }),
            dists = items.slice(1).map((v, i) => v - items[i]),
            sum = dists.reduce(function (a, b) { return a + b; })
          let avg = (sum / dists.length) / (30 * 24 * 60 * 60);
          let unit = "months";
          if (avg > 12) {
            avg /= 12;
            unit = "years";
          }
          avg = Math.floor(avg * 10) / 10;

          pubData.averageAB = avg + " " + unit;
        }

        if (jp.length > 1) {
          let items = jp.map(function (x) { return dayjs(x.releaseDate).unix() as number; }),
            dists = items.slice(1).map((v, i) => v - items[i]),
            sum = dists.reduce(function (a, b) { return a + b; })
          let avg = sum / dists.length / (30 * 24 * 60 * 60)
            ;
          let unit = "months";
          if (avg > 12) {
            avg /= 12;
            unit = "years";
          }
          avg = Math.floor(avg * 10) / 10;

          pubData.averageJP = avg + " " + unit;
        }

        if (en.length > 1) {
          let items = en.map(function (x) { return dayjs(x.releaseDate).unix() as number; }),
            dists = items.slice(1).map((v, i) => v - items[i]),
            avg = indexWeightedMean(dists) / (30 * 24 * 60 * 60)
            ;
          let unit = "months";
          if (avg > 12) {
            avg /= 12;
            unit = "years";
          }
          avg = Math.floor(avg * 10) / 10;

          pubData.weightedEN = avg + " " + unit;
        }

        if (ab.length > 1) {
          let items = ab.map(function (x) { return dayjs(x.releaseDate).unix() as number; }),
            dists = items.slice(1).map((v, i) => v - items[i]),
            avg = indexWeightedMean(dists) / (30 * 24 * 60 * 60)
            ;
          let unit = "months";
          if (avg > 12) {
            avg /= 12;
            unit = "years";
          }
          avg = Math.floor(avg * 10) / 10;

          pubData.weightedAB = avg + " " + unit;
        }

        if (jp.length > 1) {
          let items = jp.map(function (x) { return dayjs(x.releaseDate).unix() as number; }),
            dists = items.slice(1).map((v, i) => v - items[i]),
            avg = indexWeightedMean(dists) / (30 * 24 * 60 * 60)
            ;
          let unit = "months";
          if (avg > 12) {
            avg /= 12;
            unit = "years";
          }
          avg = Math.floor(avg * 10) / 10;

          pubData.weightedJP = avg + " " + unit;
        }
        setPublishRate(pubData);
      }

      if (leadtimes.length) {
        var avg = mean(leadtimes) / (30 * 24 * 60 * 60);
        var unit = "months";
        if (avg > 12) {
          avg /= 12;
          unit = "years";
        }
        avg = Math.floor(avg * 10) / 10;

        var wavg = indexWeightedMean(leadtimes) / (30 * 24 * 60 * 60);
        var wunit = "months";
        if (wavg > 12) {
          wavg /= 12;
          wunit = "years";
        }
        wavg = Math.floor(wavg * 10) / 10;

        var min = lowestArrayValue(leadtimes) / (30 * 24 * 60 * 60);
        var minunit = "months";
        if (min > 12) {
          min /= 12;
          minunit = "years";
        }
        min = Math.floor(min * 10) / 10;
        var max = highestArrayValue(leadtimes) / (30 * 24 * 60 * 60);
        var maxunit = "months";
        if (max > 12) {
          max /= 12;
          maxunit = "years";
        }
        max = Math.floor(max * 10) / 10;

        setLocalizationTime({
          average: avg + " " + unit,
          weighted: wavg + " " + wunit,
          minimum: min + " " + minunit,
          maximum: max + " " + maxunit
        });
      }
    }
    getAnswer();
  }, [props.seriesInfo.seriesId, props.seriesInfo.englishName, props.seriesInfo.publisher, props.seriesInfo.completionStatus]);

  return (
    <>
      <div>
        <HighchartsReact highcharts={Highcharts} options={options} />
      </div>
      {publishRate && <p>Publish rate--Average:{" "}
        {publishRate.averageEN && <span>EN <strong>{publishRate.averageEN}</strong> </span>}
        {publishRate.averageJP && <span>JP <strong>{publishRate.averageJP}</strong> </span>}
        {publishRate.averageAB && <span>AB <strong>{publishRate.averageAB}</strong> </span>}
        {" "}Weighted:{" "}
        {publishRate.weightedEN && <span>EN <strong>{publishRate.weightedEN}</strong> </span>}
        {publishRate.weightedJP && <span>JP <strong>{publishRate.weightedJP}</strong> </span>}
        {publishRate.weightedAB && <span>AB <strong>{publishRate.weightedAB}</strong> </span>}
      </p>}
      {localizationTime && <p>Loc time --
        Average: <strong>{localizationTime.average}</strong>{" "}
        Weighted: <strong>{localizationTime.weighted}</strong>{" "}
        Min: <strong>{localizationTime.minimum}</strong>{" "}
        Max: <strong>{localizationTime.maximum}</strong>
      </p>}
    </>
  );
}

export default GraphSample;