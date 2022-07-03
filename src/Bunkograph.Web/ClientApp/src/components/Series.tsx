import { useEffect, useState } from "react";
import { Input } from "reactstrap";
import GraphSample from "./BookSeriesGraph";

export interface ISeriesInfo {
  seriesId: number,
  englishName: string,
  publisher?: string
}

const populateSeriesData = async () => {
  const response = await fetch('api/series');
  const data = await response.json();

  return data as ISeriesInfo[];
}

const Series: React.FC = () => {
  const [seriesInfos, setSeriesInfos] = useState<ISeriesInfo[]>();
  const [seriesInfo, setSeriesInfo] = useState<ISeriesInfo | undefined>();

  const setSeries = (e: React.ChangeEvent<HTMLInputElement>) => {
    const seriesId = parseInt(e.target.value);
    if (seriesId && seriesInfos) {
      for (const seriesInfo of seriesInfos)
      {
        if (seriesInfo.seriesId === seriesId) {
          setSeriesInfo(seriesInfo);
        }
      }
    }
    else {
      setSeriesInfo(undefined);
    }
  }

  useEffect(() => {
    const getAnswer = async () => {
      const series = await populateSeriesData();
      setSeriesInfos(series);
    }
    getAnswer();
  }, []);

  return (
    <>
      {
        seriesInfos &&
        <Input type="select" aria-label="Default select example" onChange={setSeries}>
            {
              !seriesInfo && <option value={0}>Please select a series</option>
            }
            {
              seriesInfos.map((value: ISeriesInfo) => <option key={value.seriesId} value={value.seriesId}>{value.englishName}</option>)
            }
        </Input>
      }
      {
        seriesInfo && <GraphSample seriesInfo={seriesInfo} />
      }
    </>
  )
}

export default Series;
