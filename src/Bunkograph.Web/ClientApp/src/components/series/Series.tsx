import { useEffect, useState } from "react";
import { Link, Outlet } from "react-router-dom";
import Loading from "../Loading";

export interface ISeriesInfo {
  seriesId: number,
  originalName: string,
  englishName: string,
  licenses: ISeriesLicense[]
  originalLicense?: ISeriesLicense
  englishLicense?: ISeriesLicense
}

interface IPublisher {
  name: string
}

interface ILangugage {
  languageId: string
}

interface ISeriesLicense {
  seriesLicenseId: number,
  language: ILangugage,
  completionStatus?: CompletionStatus,
  publisher: IPublisher
}

export enum CompletionStatus {
  Ongoing,
  OneShot,
  Completed,
  Cancelled,
}

async function populateSeriesData() {
  const response = await fetch('api/series');
  const data = await response.json();

  const result = data as ISeriesInfo[];
  for (const series of result) {
    for (const license of series.licenses) {
      if (license.language.languageId === "jp") {
        series.originalLicense = license;
      } else if (license.language.languageId === "en") {
        series.englishLicense = license;
      }
    }
  }

  return result;
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
  }, []);

  return (
    <>
      {
        !seriesInfos && <Loading />
      }
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
                  {
                    value.licenses.map((licenseValue) => <div key={licenseValue.seriesLicenseId}>
                      {licenseValue.language.languageId}: {(licenseValue.completionStatus && CompletionStatus[licenseValue.completionStatus]) || "Ongoing"}
                      <br /></div>)
                  }
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
                  {" "}
                  <Link
                    to={`/series/${value.seriesId}/editvolumes`}
                  >
                    Volumes
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
