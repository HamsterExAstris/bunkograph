import { useState } from "react";
import GraphSample from "./BookSeriesGraph";

export interface ISeriesInfo {
  seriesId: number,
  series: string,
  publisher?: string
}

const Series: React.FC = () => {
  const [seriesInfo, setSeriesInfo] = useState<ISeriesInfo>({
    seriesId: 1,
    series: "The Saga of Tanya the Evil"
  });

  return (
    <>
      {
        seriesInfo && <GraphSample seriesInfo={seriesInfo} />
      }
    </>
  )
}

export default Series;
