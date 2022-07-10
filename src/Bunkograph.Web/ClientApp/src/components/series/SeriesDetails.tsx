import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import GraphSample from "../BookSeriesGraph";
import { ISeriesInfo } from "./Series";

async function populateSeriesData(seriesId: number) {
  const response = await fetch('api/series/' + seriesId);
  const data = await response.json();

  return data as ISeriesInfo;
}

const SeriesDetails: React.FC = () => {
  const [seriesInfo, setSeriesInfo] = useState<ISeriesInfo | undefined>();

  const params = useParams();

  useEffect(() => {
    const getAnswer = async () => {
      // If the URL specifies a specific series, load the data into state for rendering.
      const seriesId = params.seriesId ? parseInt(params.seriesId) : undefined;
      if (seriesId) {
        // Get the series data from the API.
        const series = await populateSeriesData(seriesId);
        setSeriesInfo(series);
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
        seriesInfo && <GraphSample seriesInfo={seriesInfo} />
      }
    </>
  )
}

export default SeriesDetails;
