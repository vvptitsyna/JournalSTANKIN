import {useNavigate, useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import API from "../Core/api";
import {
    getAdminSubGroupsAction,
    getAdminVersionAction,
    selectedAdminGroupVersionAction,
    selectedAdminSubGroupAction
} from "../Core/adminGroupsReducer";
import {useDispatch, useSelector} from "react-redux";
import Navigation from "../HardComponents/Navigation";
import Footer from "../HardComponents/Footer";

import '../css/version.css'
import ChangeGroup from "../HardComponents/ChangeGroup";
import List from "../SimpleComponents/List";
import {selectUserAction} from "../Core/usersReducer";
import AddSubGroup from "../HardComponents/AddSubGroup";
function EditVersionPage() {
    const dispatch = useDispatch();
    const versionId = useSelector(store =>store.adminGroups.selectedVersion)
    const group = useSelector(store =>store.adminGroups.version)
    const [refreshGroup, setRefreshGroup] = useState(false);
    const subGroups = useSelector(store => store.adminGroups.subGroups === undefined ? [{name: 'нет', comment: 'нет'}] : store.adminGroups.subGroups)
    const navigate = useNavigate();

    useEffect(() => {
        API.get(`Administrator/Group/getGroupWithVersionInfo?groupWithVersionId=${versionId}`, { withCredentials: true })
            .then((res) => {
                dispatch(getAdminVersionAction(res.data));
                dispatch(getAdminSubGroupsAction(res.data.subgroups.$values));
                console.log(res.data);
            })
            .catch((res) => {
                console.log(res.error);
            });
    },[dispatch,refreshGroup])

    const handleSelectVersion = (id) => {
        dispatch(selectedAdminSubGroupAction(id));
        navigate(`/edit-subgroup`);
    }

    const handleRefreshGroup = () => {
        setRefreshGroup(!refreshGroup);
        console.log('Я ОБНОВИЛАСЬ')
    }
    return (
        <div className="wrapper-version">
            <Navigation/>
            <div className="version">
                <div className="versionInfo">
                        <h1>главная группа: {group.mainGroupName}</h1>
                        <h1>группа: {group.name}</h1>
                        <h1>дата создания: {group.dateOfCreateon}</h1>
                        <h1>версия: {group.version}</h1>
                    </div>
                <ChangeGroup onUserChanged={handleRefreshGroup}/>
                <div className="wrapper-list">
                    <h1>Список подгрупп:</h1>
                    <List onRowClick={(id) => handleSelectVersion(id)} children = {subGroups} fieldsToShow={['name', 'comment']} fieldsHeader={['Название подгруппы', 'Комментарий']}/>
                </div>
                <AddSubGroup onUserChanged={handleRefreshGroup} id={versionId}/>
            </div>
            <Footer/>
        </div>
    );
}

export default EditVersionPage;