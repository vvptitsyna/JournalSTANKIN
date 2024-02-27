import {useDispatch, useSelector} from "react-redux";
import {useEffect, useState} from "react";
import API from "../Core/api";
import {getAdminSubjectsAction} from "../Core/adminSubjectsReducer";
import {getAdminSemestersAction} from "../Core/adminSemestersReducer";
import AddSubject from "../HardComponents/AddSubject";
import List from "../SimpleComponents/List";
import AddSemester from "../HardComponents/AddSemester";

function SemestersPage () {

    const dispatch = useDispatch();
    const semesters = useSelector(store => store.adminSemesters.semesters)
    const [refreshSemesters, setRefreshSemesters] = useState(false);

    useEffect(() => {
        API.get(`/Administrator/Semester/getSemesters`, { withCredentials: true })
            .then(res => {
                dispatch(getAdminSemestersAction(res.data.$values))
                console.log(res.data.$values)
                console.log(semesters)
            }).catch(res => {
            console.log(res.error);
        })
    }, [dispatch,refreshSemesters]);

    const handleRefreshSemesters = () => {
        setRefreshSemesters(!refreshSemesters);
        console.log('Я ОБНОВИЛАСЬ')
    };

    return (
        <div className="wrapper-semesters">
            <AddSemester onUserChanged={handleRefreshSemesters}/>
            <div className="wrapper-list">
                <h1>Список семестров:</h1>
                <List children = {semesters} fieldsToShow={['year', 'season']} fieldsHeader={['Год', 'Сезон']}/>
            </div>
        </div>
    );
}
export default SemestersPage;