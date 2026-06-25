import { Routes, Route } from "react-router-dom";
import Workspace from "./pages/WorkspacePage";
import Home from "./pages/HomePage";

/** A router for the main application. */
export default function App() {
    return (
        <Routes>
            <Route index element={<Home />} />
            <Route path="/workspace/:id" element={<Workspace />} />
        </Routes>
    );
};