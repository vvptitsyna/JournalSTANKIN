import {useDispatch, useSelector} from "react-redux";
import API from "../Core/api";
import {getSubjectsAction} from "../Core/semestersReducer";
import {selectSubjectAction} from "../Core/subjectsReducer";
import {useEffect, useState} from "react";
import {setUserAction} from "../Core/usersReducer";
import Input from "../SimpleComponents/Input";
import Select from 'react-select'
import Button from "../SimpleComponents/Button";

const ChangeUser = ({
                        onUserChanged, ...attrs
                    }) => {

    const dispatch = useDispatch();
    const user = useSelector(state => state.users.user)
    const selectedUser = useSelector(state => state.users.selectedUser)

    const [userData, setUserData] = useState({
        id: user.id,
        userName: user.userName,
        password: '',
        needChangePassword: null,
        role: user.role,
        comment: user.comment
    });

    useEffect(() => {
        setUserData({
            id: user.id,
            userName: user.userName,
            password: '',
            needChangePassword: null,
            role: user.role,
            comment: user.comment,
        });
    }, [user])

    const handleUserDataChange = (e) => {
        setUserData({
            ...userData,
            [e.target.id]: e.target.value
        })}

    useEffect(() => {
        API.get(`Administrator/User/getUserInfo?userId=${selectedUser}`, { withCredentials: true })
            .then(res => {
                dispatch(setUserAction(res.data));
            });
    }, [dispatch, selectedUser]);

    const options = [
        { value: 'Teacher', label: 'Преподаватель' },
        { value: 'Support', label: 'Персонал поддержки' },
        { value: 'Administrator', label: 'Администратор' }
    ]

    const selectedOption = options.find(option => option.value === userData.role);

    const handleSubmit = (e) => {
        e.preventDefault();

        API.post('Administrator/User/editUser', userData, { withCredentials: true } )
            .then(res => {
                console.log (userData);
                console.log (res.data);
                onUserChanged();
            }) .catch(res => {
            console.log(res.error);
            console.log (res);
        })
    }



    return (

            <form className="change-form" onSubmit={e => handleSubmit(e)}>
                <h1>Изменить пользователя</h1>
                <Input id="userName" value={userData.userName} onChange={e => handleUserDataChange(e)} type="text" className="add"/>
                <Input id="password" value={userData.password} onChange={e => handleUserDataChange(e)} type="text" className="add"/>
                <Input id="needChangePassword" value={userData.needChangePassword} onChange={e => handleUserDataChange(e)} type="checkbox" label="Нужно изменить пароль"/>
                <Select id="role" name="role" options={options} value={selectedOption} onChange={(selectedOption) => setUserData({...userData, role: selectedOption.value})}/>
                <Input id="comment" value={userData.comment} onChange={e => handleUserDataChange(e)} type="text" className="add"/>
                <Button children="Изменить" type="submit"/>
            </form>
    );
};

export default ChangeUser;