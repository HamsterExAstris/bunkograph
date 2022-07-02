import dayjs from 'dayjs'
import Highcharts, { PointOptionsObject } from 'highcharts';
import HighchartsReact from 'highcharts-react-official';

export interface IGraphSampleProps {
  series: string,
  publisher: string
}

interface IVolumeInfo {
  release: number, // unix time * 1000
  volume: number,
  label: string
}

interface IPoint extends PointOptionsObject {
  label: string
}

const dataJPRaw: IVolumeInfo[] = [
  {
    release: dayjs("2013-10-31").unix() * 1000,
    volume: 1,
    label: "1"
  },
  {
    release: dayjs("2014-05-31").unix() * 1000,
    volume: 2,
    label: "2"
  },
  {
    release: dayjs("2014-11-29").unix() * 1000,
    volume: 3,
    label: "3"
  },
  {
    release: dayjs("2015-06-29").unix() * 1000,
    volume: 4,
    label: "4"
  },
  {
    release: dayjs("2016-01-30").unix() * 1000,
    volume: 5,
    label: "5"
  },
  {
    release: dayjs("2016-07-16").unix() * 1000,
    volume: 6,
    label: "6"
  },
  {
    release: dayjs("2016-12-28").unix() * 1000,
    volume: 7,
    label: "7"
  },
  {
    release: dayjs("2017-06-30").unix() * 1000,
    volume: 8,
    label: "8"
  },
  {
    release: dayjs("2018-01-12").unix() * 1000,
    volume: 9,
    label: "9"
  },
  {
    release: dayjs("2018-09-29").unix() * 1000,
    volume: 10,
    label: "10"
  },
  {
    release: dayjs("2019-02-20").unix() * 1000,
    volume: 11,
    label: "11"
  },
  {
    release: dayjs("2020-02-20").unix() * 1000,
    volume: 12,
    label: "12"
  },
  {
    release: dayjs("2023-01-01").unix() * 1000,
    volume: 13,
    label: "Next?"
  }
]

const dataENRaw: IVolumeInfo[] = [
  {
    release: dayjs("2017-12-19").unix() * 1000,
    volume: 1,
    label: "1"
  },
  {
    release: dayjs("2018-03-20").unix() * 1000,
    volume: 2,
    label: "2"
  },
  {
    release: dayjs("2018-07-31").unix() * 1000,
    volume: 3,
    label: "3"
  },
  {
    release: dayjs("2018-11-27").unix() * 1000,
    volume: 4,
    label: "4"
  },
  {
    release: dayjs("2019-03-26").unix() * 1000,
    volume: 5,
    label: "5"
  },
  {
    release: dayjs("2019-07-30").unix() * 1000,
    volume: 6,
    label: "6"
  },
  {
    release: dayjs("2020-06-23").unix() * 1000,
    volume: 7,
    label: "7"
  },
  {
    release: dayjs("2020-12-22").unix() * 1000,
    volume: 8,
    label: "8"
  },
  {
    release: dayjs("2022-01-18").unix() * 1000,
    volume: 9,
    label: "9"
  },
  {
    release: dayjs("2022-05-24").unix() * 1000,
    volume: 10,
    label: "10"
  },
  {
    release: dayjs("2022-10-18").unix() * 1000,
    volume: 11,
    label: "11"
  },
  {
    release: dayjs("2023-04-01").unix() * 1000,
    volume: 12,
    label: "Next?"
  }
]

const dataABRaw: IVolumeInfo[] = [
  {
    release: dayjs("2021-11-16").unix() * 1000,
    volume: 1,
    label: "1"
  },
  {
    release: dayjs("2022-01-11").unix() * 1000,
    volume: 2,
    label: "2"
  },
  {
    release: dayjs("2022-03-15").unix() * 1000,
    volume: 3,
    label: "3"
  },
  {
    release: dayjs("2022-07-05").unix() * 1000,
    volume: 4,
    label: "4"
  },
  {
    release: dayjs("2022-10-11").unix() * 1000,
    volume: 5,
    label: "5"
  },
  {
    release: dayjs("2023-01-15").unix() * 1000,
    volume: 6,
    label: "Next?"
  }
]

const mapToPoint = function (v: IVolumeInfo): IPoint {
  return {
    x: v.release + 1000 * 60 * 60 * 12,
    y: v.volume,
    z: v.volume,
    label: v.label
  }
}

const dataJP: IPoint[] = dataJPRaw.map(mapToPoint);
const dataEN: IPoint[] = dataENRaw.map(mapToPoint);
const dataENAB: IPoint[] = dataABRaw.map(mapToPoint);

const GraphSample: React.FC<IGraphSampleProps> = (props) => {
  const options: Highcharts.Options = {
    title: {
      text: '<h3>' + props.series + ' <small class="text-muted">' + props.publisher + '</small></h3>',
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
        data: dataENAB,
        dataLabels: {
          enabled: true,
          format: "{point.label}",
        }
      }
    ]
  };

  return (
    <div>
      <HighchartsReact highcharts={Highcharts} options={options} />
    </div>
  );
}

export default GraphSample;