import { Home } from "./components/Home";
import Series from "./components/Series";

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
    element: <Series />
  },
];

export default AppRoutes;
