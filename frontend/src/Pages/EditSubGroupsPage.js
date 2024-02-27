import {useNavigate, useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import API from "../Core/api";
import {
    getAdminSubGroupAction,
    getAdminSubGroupsAction, getAdminSubgroupStudentsAction,
    getAdminVersionAction,
    selectedAdminGroupVersionAction,
    selectedAdminSubGroupAction
} from "../Core/adminGroupsReducer";
import {useDispatch, useSelector} from "react-redux";
import Navigation from "../HardComponents/Navigation";
import Footer from "../HardComponents/Footer";

import '../css/subgroup.css'
import List from "../SimpleComponents/List";
import AddNewStudent from "../HardComponents/AddNewStudent";
import AddStudent from "../HardComponents/AddStudent";
function EditSubGroupsPage() {
    const dispatch = useDispatch();
    const subGroupId = useSelector(store =>store.adminGroups.selectedSubGroup)
    const subGroup = useSelector(store =>store.adminGroups.subGroup)
    const [refreshSubGroup, setRefreshSubGroup] = useState(false);
    const students = useSelector(store => store.adminGroups.students === undefined ? [{name: 'нет', comment: 'нет'}] : store.adminGroups.students)

    useEffect(() => {
        API.get(`Administrator/Group/getSubgroupInfo?subgroupId=${subGroupId}`, { withCredentials: true })
            .then((res) => {
                dispatch(getAdminSubGroupAction(res.data));
                dispatch(getAdminSubgroupStudentsAction(res.data.students.$values))
                console.log(res.data.students.$values);
                console.log(students);
            })
            .catch((res) => {
                console.log(res.error);
            });
    },[dispatch,refreshSubGroup])


    const handleRefreshSubGroup = () => {
        setRefreshSubGroup(!refreshSubGroup);
        console.log('Я ОБНОВИЛАСЬ')
    }
    return (
        <div className="wrapper-subgroup">
            <Navigation/>
            <div className="subgroup">
                <div className="groupInfo">
                    <h1>главная группа: {subGroup.mainGroupName}</h1>
                    <h1>группа: {subGroup.name}</h1>
                    <h1>версия: {subGroup.version}</h1>
                </div>
                <AddNewStudent onUserChanged={handleRefreshSubGroup} id={subGroupId}/>
                <div className="wrapper-list">
                    <h1>Список студентов:</h1>
                    <List children = {students} fieldsToShow={['name', 'comment']} fieldsHeader={['Название подгруппы', 'Комментарий']}/>
                </div>
                <AddStudent onUserChanged={handleRefreshSubGroup}/>
            </div>
            <Footer/>
        </div>
    );
}

export default EditSubGroupsPage;