import { Home } from "./components/Home";
import Series from "./components/series/Series";
import SeriesBooks from "./components/series/SeriesBooks";
import SeriesDetails from "./components/series/SeriesDetails";
import SeriesEdit from "./components/series/SeriesEdit";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/series',
    element: <Series />
  },
  {
    path: '/series/:seriesId',
    //requireAuth: true,
    element: <SeriesDetails />
  },
  {
    path: '/series/:seriesId/edit',
    //requireAuth: true,
    element: <SeriesEdit />
  },
  {
    path: '/series/:seriesId/editvolumes',
    //requireAuth: true,
    element: <SeriesBooks />
  },
];

export default AppRoutes;
