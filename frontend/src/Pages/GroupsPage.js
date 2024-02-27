import {useDispatch, useSelector} from "react-redux";
import {useEffect, useState} from "react";
import API from "../Core/api";
import {getAdminSemestersAction} from "../Core/adminSemestersReducer";
import {getAdminRelationsAction} from "../Core/adminRelationsReducer";
import List from "../SimpleComponents/List";
import AddGroup from "../HardComponents/AddGroup";
import {
    getAdminGroupsAction, getAdminVersionAction,
    getAdminVersionsAction,
    selectedAdminGroupAction,
    selectedAdminGroupVersionAction
} from "../Core/adminGroupsReducer";
import {selectUserAction} from "../Core/usersReducer";
import Button from "../SimpleComponents/Button";
import {useNavigate} from "react-router-dom";

function GroupsPage () {

    const dispatch = useDispatch();
    const groups = useSelector(store => store.adminGroups.groups)
    const versions = useSelector( store=> store.adminGroups.versions)
    const selectedGroup = useSelector( store=> store.adminGroups.selectedGroup)
    const versionId=useSelector(store =>store.adminGroups.selectedVersion)
    const [refreshUsers, setRefreshUsers] = useState(false);
    const [refreshVersions, setRefreshVersions] = useState(false);
    const navigate = useNavigate();

    useEffect(() => {
        API.get(`Administrator/Group/getGroups`, { withCredentials: true })
            .then(res => {
                dispatch(getAdminGroupsAction(res.data.$values))
                dispatch(selectedAdminGroupAction(res.data.$values[0].id))
            }).catch(res => {
            console.log(res.error);
        })
    }, [dispatch,refreshUsers]);

    useEffect(() => {
        API.get(`Administrator/Group/getGroupVersions?groupId=${selectedGroup}`, { withCredentials: true })
            .then(res => {
                dispatch(getAdminVersionsAction(res.data.$values))
            }).catch(res => {
            console.log(res.error);
        })
    }, [dispatch,selectedGroup, refreshVersions]);

    const handleRefreshUsers = () => {
        setRefreshUsers(!refreshUsers);
        console.log('Я ОБНОВИЛАСЬ')
    };

    const handleNewVersionClick = () => {
        API.get(`Administrator/Group/createNewVersionOfGroup?groupId=${selectedGroup}`, { withCredentials: true })
            .then((res) => {
                setRefreshVersions(!refreshVersions);
            })
            .catch((res) => {
                console.log(res.error);
            });
    };

    const handleSelectVersion = (id) => {
        dispatch(selectedAdminGroupVersionAction(id));
        navigate(`/edit-version`);
    }

    return (
        <div className="wrapper-groups">
            <AddGroup onUserChanged={handleRefreshUsers}/>
            <div className="wrapper-list">
                <h1>Список групп:</h1>
                <List onRowClick={(id) => dispatch(selectedAdminGroupAction(id))} children = {groups} fieldsToShow={['name', 'comment']} fieldsHeader={['Название группы', 'Комментарий']}/>
            </div>
            <div className="wrapper-versions">
                <h1>Список версий:</h1>
                <List onRowClick={(id) => handleSelectVersion(id)} children = {versions} fieldsToShow={['name', 'version']} fieldsHeader={['Название группы', 'Версия']}/>
                <Button onClick={handleNewVersionClick} children={"Создать новую версию"}/>
            </div>
        </div>
    );
}
export default GroupsPage;