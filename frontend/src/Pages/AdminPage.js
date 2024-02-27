import Navigation from "../HardComponents/Navigation";
import {useLocation} from "react-router-dom";
import SubjectsPage from "./SubjectsPage";
import UsersPage from "./UsersPage";
import SemestersPage from "./SemestersPage";
import GroupsPage from "./GroupsPage";
import RelationsPage from "./RelationsPage";
import LogsPage from "./LogsPage";
import classNames from "classnames";

import '../css/admin-pages.css'
import Footer from "../HardComponents/Footer";

function AdminPage () {

    const location = useLocation();
    const searchParams = new URLSearchParams(location.search);
    const option = searchParams.get('page');

    const pageStyles = {
        "subjects": "wrapper-subjects",
        "users": "wrapper-users",
        "semesters": "wrapper-semesters",
        "groups": "wrapper-groups",
        "relations": "wrapper-relations",
        "logs": "wrapper-logs"
    };

    return (
        <div className="wrapper-admin">
            <Navigation/>
            {option === 'subjects' && <SubjectsPage />}
            {option === 'users' && <UsersPage />}
            {option === 'semesters' && <SemestersPage />}
            {option === 'groups' && <GroupsPage />}
            {option === 'relations' && <RelationsPage />}
            {option === 'logs' && <LogsPage />}
            <Footer/>
        </div>
    );
}
export default AdminPage;