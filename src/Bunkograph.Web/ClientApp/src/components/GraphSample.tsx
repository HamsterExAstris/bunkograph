import dayjs from 'dayjs'
import Highcharts, { PointOptionsObject } from 'highcharts';
import highchartsAccessibility from "highcharts/modules/accessibility";
import HighchartsReact from 'highcharts-react-official';
import { useEffect, useState } from 'react';

// Initialize Highcharts for accessibility.
highchartsAccessibility(Highcharts);

export interface IGraphSampleProps {
  series: string,
  publisher: string
}

interface IVolumeInfo {
  release: number, // unix time * 1000
  volumeNumber: number,
  label: string
}

interface IPoint extends PointOptionsObject {
  label: string
}

const populateVolumeData = async (language: string) => {
  const response = await fetch('volumes/' + language);
  const data = await response.json();

  return data as IVolumeInfo[];
}

const mapToPoint = function (v: IVolumeInfo): IPoint {
  return {
    x: v.release + 1000 * 60 * 60 * 12,
    y: v.volumeNumber,
    z: v.volumeNumber,
    label: v.label
  }
}

const buildOptions = function (series: string, publisher: string, dataJP?: IPoint[], dataEN?: IPoint[], dataAB?: IPoint[]): Highcharts.Options {
  return {
    title: {
      text: '<h3>' + series + ' <small class="text-muted">' + publisher + '</small></h3>',
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
        color: '#dc3545', // Red
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
        color: "#0d6efd",
        data: dataJP,
        dataLabels: {
          enabled: true,
          format: "{point.label}",
        }
      },
      {
        type: "spline",
        name: "EN",
        color: "#198754",
        data: dataEN,
        dataLabels: {
          enabled: true,
          format: "{point.label}",
        }
      },
      {
        type: "spline",
        name: "AB",
        color: "#6f42c1",
        data: dataAB,
        dataLabels: {
          enabled: true,
          format: "{point.label}",
        }
      }
    ]
  }
}

const GraphSample: React.FC<IGraphSampleProps> = (props) => {
  const [options, setOptions] = useState<Highcharts.Options>(buildOptions(props.series, props.publisher));

  useEffect(() => {
    const getAnswer = async () => {
      const volumesJP = await populateVolumeData("jp");
      const volumesEN = await populateVolumeData("en");
      const volumesAB = await populateVolumeData("ab");

      const dataJP: IPoint[] = volumesJP.map(mapToPoint);
      const dataEN: IPoint[] = volumesEN.map(mapToPoint);
      const dataENAB: IPoint[] = volumesAB.map(mapToPoint);

      setOptions(buildOptions(props.series, props.publisher, dataJP, dataEN, dataENAB));
    }
    getAnswer();
  }, [props.series, props.publisher]);

  return (
    <div>
      <HighchartsReact highcharts={Highcharts} options={options} />
    </div>
  );
}

export default GraphSample;