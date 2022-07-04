import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
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

  const navigate = useNavigate();
  const params = useParams();

  const setSeries = (e: React.ChangeEvent<HTMLInputElement>) => {
    const seriesId = parseInt(e.target.value);
    if (seriesId) {
      navigate('/series/' + seriesId);
    }
    else {
      navigate('/series');
    }
  }

  useEffect(() => {
    const getAnswer = async () => {
      // Get the data for all series from the API.
      const series = await populateSeriesData();
      setSeriesInfos(series);

      // If the URL specifies a specific series, load the data into state for rendering.
      const seriesId = params.seriesId ? parseInt(params.seriesId) : undefined;
      if (seriesId) {
        for (const seriesInfo of series) {
          if (seriesInfo.seriesId === seriesId) {
            setSeriesInfo(seriesInfo);
          }
        }
      }
      else {
        setSeriesInfo(undefined);
      }
    }
    getAnswer();
  }, [params.seriesId]);

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
