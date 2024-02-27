import Input from "../SimpleComponents/Input";
import React, {useEffect, useState} from "react";
import API from "../Core/api";
import Button from "../SimpleComponents/Button";
import {getUsersAction} from "../Core/usersReducer";
import {useDispatch} from "react-redux";

const AddUser = ({onUserChanged}) => {

    const dispatch = useDispatch();
    const [usernameEmpty, setUsernameEmpty] = useState(false);
    const [passwordEmpty, setPasswordEmpty] = useState(false);
    const [roleEmpty, setRoleEmpty] = useState(false);
    const [formValid, setFormValid] = useState (false);
    const [username, setUsername] = useState();
    const [password, setPassword] = useState();
    const [role, setRole] = useState('Teacher');
    const [comment, setComment] = useState('');

    const handleSubmit = (e) => {
        e.preventDefault();

        const data = {
            username: username,
            password: password,
            role: role,
            comment: comment,
        }

        API.post('Administrator/User/addUser', data, { withCredentials: true } )
            .then(res => {
                console.log (res.data);
                console.log (data);
            }) .catch(res => {
            console.log(res.error);
            console.log (res);
        })

        API.get(`/Administrator/User/getUsersInfo`, { withCredentials: true })
            .then(res => {
                dispatch(getUsersAction(res.data.$values))
                onUserChanged()
            }).catch(res => {
            console.log(res.error);
        }, [dispatch])
    }

    useEffect( () => {
            if (usernameEmpty || passwordEmpty || roleEmpty)
                setFormValid(false)
            else
                setFormValid(true)
        },[usernameEmpty, passwordEmpty, roleEmpty])

    const loginHandler = (e) => {
        setUsername(e.target.value);
        if (!e.target.value){
            setUsernameEmpty(true)
        }
        else setUsernameEmpty(false)
    }
    const passwordHandler = (e) => {
        setPassword(e.target.value);
        if (!e.target.value){
            setPasswordEmpty(true)
        }
        else setPasswordEmpty(false)
    }

    const commentHandler = (e) => {
        setComment(e.target.value);
    }

    const roleHandler = (e) => {
        setRole(e.target.value);
        console.log(role)
        if (!e.target.value){
            setRoleEmpty(true)
        }
        else setRoleEmpty(false)
    }

    return (
            <div className="wrapper-add">
                <form onSubmit={e => handleSubmit(e)} className="add-form">
                    <h1>Добавить пользователя</h1>
                    <Input value={username} onChange={e => loginHandler(e)} id="username" type="text"  placeholder="ФИО" className="add"/>
                    <Input value={password} onChange={e => passwordHandler(e)} id="password" type="password"  placeholder="ПАРОЛЬ" className="add"/>
                    <Input value={comment} onChange={e => commentHandler(e)} id="comment" type="text" placeholder="Комментарий" className="add"/>
                    <div className="role check" onChange={e => roleHandler(e)}>
                        <Input type="radio" value="Teacher" name="role" checked={role === "Teacher"} label="Преподаватель"/>
                        <Input type="radio" value="Support" name="role" label="Персонал поддержки"/>
                        <Input type="radio" value="Administrator" name="role" label="Администратор"/>
                    </div>
                    <Button children="Создать" type="submit" disabled={!formValid}/>
                </form>
            </div>
    );
};

export default AddUser;