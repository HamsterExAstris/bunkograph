import { useEffect, useState } from "react";
import { Link, Outlet } from "react-router-dom";

export interface ISeriesInfo {
  seriesId: number,
  originalName: string,
  englishName: string,
  publisher?: string,
  completionStatus?: CompletionStatus
}

export enum CompletionStatus {
  None,
  OneShot,
  Completed,
  Cancelled,
}

async function populateSeriesData() {
  const response = await fetch('api/series');
  const data = await response.json();

  return data as ISeriesInfo[];
}

const Series: React.FC = () => {
  const [seriesInfos, setSeriesInfos] = useState<ISeriesInfo[]>();

  useEffect(() => {
    const getAnswer = async () => {
      // Get the data for all series from the API.
      const series = await populateSeriesData();
      setSeriesInfos(series);
    }
    getAnswer();
  });

  return (
    <>
      <p>
        <a asp-action="Create" href="https://www.google.com" target="_blank" rel="noreferrer">Create New</a>
      </p>
      <table className="table">
        <thead>
          <tr>
            <th>
                Original Name
            </th>
            <th>
                English Name
            </th>
            <th>
                Completion Status
            </th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {
            seriesInfos?.map((value) =>
              <tr key={value.seriesId}>
                <td>
                  {value.originalName}
                </td>
                <td>
                  {value.englishName}
                </td>
                <td>
                  {value.completionStatus && CompletionStatus[value.completionStatus]}
                </td>
                <td>
                  <Link
                    to={`/series/${value.seriesId}/edit`}
                  >
                    Edit
                  </Link>
                  {" "}
                  <Link
                    to={`/series/${value.seriesId}`}
                  >
                    Details
                  </Link>
                  { /* <a asp-action="Delete" asp-route-id="@item.SeriesId">Delete</a> */}
                </td>
              </tr>
            )
          }
        </tbody>
      </table>
      <Outlet />
    </>
  )
}

export default Series;
