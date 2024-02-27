import {useDispatch, useSelector} from "react-redux";
import {useEffect, useState} from "react";
import API from "../Core/api";
import {getUsersAction, selectUserAction} from "../Core/usersReducer";
import {getAdminSubjectsAction} from "../Core/adminSubjectsReducer";
import List from "../SimpleComponents/List";
import AddSubject from "../HardComponents/AddSubject";

function SubjectPage () {

    const dispatch = useDispatch();
    const subjects = useSelector(store => store.adminSubjects.subjects)
    const [refreshUsers, setRefreshUsers] = useState(false);

    useEffect(() => {
        API.get(`/Administrator/Subject/getSubjects`, { withCredentials: true })
            .then(res => {
                dispatch(getAdminSubjectsAction(res.data.$values))
            }).catch(res => {
            console.log(res.error);
        })
    }, [dispatch,refreshUsers]);

    const handleRefreshUsers = () => {
        setRefreshUsers(!refreshUsers);
        console.log('Я ОБНОВИЛАСЬ')
    };

    return (
        <div className="wrapper-subjects">
            <AddSubject onUserChanged={handleRefreshUsers}/>
            <div className="wrapper-list">
                <h1>Список предметов:</h1>
                <List children = {subjects} fieldsToShow={['name', 'comment']} fieldsHeader={['Предмет', 'Комментарий']}/>
            </div>

        </div>
    );
}
export default SubjectPage;