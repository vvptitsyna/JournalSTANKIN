import {useDispatch, useSelector} from "react-redux";
import {useEffect, useState} from "react";
import API from "../Core/api";
import {getUsersAction, selectUserAction} from "../Core/usersReducer";
import List from "../SimpleComponents/List";
import AddUser from "../HardComponents/AddUser";
import ChangeUser from "../HardComponents/ChangeUser";

function UsersPage () {

    const dispatch = useDispatch();
    const users = useSelector(store => store.users.users)
    const [refreshUsers, setRefreshUsers] = useState(false);
    const userId = useSelector(store=> store.users.selectedUser)

    useEffect(() => {
        API.get(`/Administrator/User/getUsersInfo`, { withCredentials: true })
            .then(res => {
                dispatch(getUsersAction(res.data.$values))
                dispatch(selectUserAction(res.data.$values[0].id))
            }).catch(res => {
            console.log(res.error);
        })
    }, [dispatch,refreshUsers]);

    const handleRefreshUsers = () => {
        setRefreshUsers(!refreshUsers);
        console.log('Я ОБНОВИЛАСЬ')
    };

    return (
        <div className="wrapper-users">
                <AddUser onUserChanged={handleRefreshUsers}/>
            <div className="wrapper-list">
                <h1>Список пользователей:</h1>
                <List onRowClick={(id) => dispatch(selectUserAction(id))} children = {users} fieldsToShow={['userName', 'role', 'comment']} fieldsHeader={['Имя пользователя', 'Роль', 'Комментарий']}/>
            </div>
            <div className="wrapper-change">
                <ChangeUser onUserChanged={handleRefreshUsers}/>
            </div>
        </div>
    );
}
export default UsersPage;