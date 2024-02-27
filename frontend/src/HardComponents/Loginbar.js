import React, {useEffect, useState} from 'react'
import axios from "axios";
import API from "../Core/api"
import { Cookies } from 'react-cookie'
import { useNavigate } from "react-router-dom";
import Input from "../SimpleComponents/Input"
import Logo from '../SimpleComponents/Logo'
import Button from '../SimpleComponents/Button'


import '../css/loginbar.css'

const Loginbar = () => {
    const navigate = useNavigate();
    const [usernameEmpty, setUsernameEmpty] = useState(false);
    const [passwordEmpty, setPasswordEmpty] = useState(false);
    const [formValid, setFormValid] = useState (false);
    const [username, setUsername] = useState();
    const [password, setPassword] = useState();

    const handleSubmit = (e) => {
        e.preventDefault();

        const data = {
            username: username,
            password: password,
        }

        API.post('Authorization/login', data, { withCredentials: true } )
            .then(res => {
                console.log (res.data);
                switch (res.data) {
                    case 'Teacher':
                        navigate('/main')
                        break;
                    case 'Administrator':
                        navigate('/administration')
                        break;
                    case 'Support':
                        navigate('/support')
                        break;
                }
        }) .catch(res => {
            console.log(res.error);
            console.log (res);
        })
    }

    useEffect( () => {
        if (usernameEmpty || passwordEmpty)
            setFormValid(false)
        else
            setFormValid(true)
        }

    )

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

    return (
        <div className="active-login">
            <form onSubmit={e => handleSubmit(e)} className="auth-form">
                <Logo width="100%" height="127px"/>
                <h1 className="login-h1">Войти в модульный журнал</h1>
                <Input value={username} onChange={e => loginHandler(e)} id="username" type="text"  placeholder="ФИО" className="input-auth"/>
                <Input value={password} onChange={e => passwordHandler(e)} id="password" type="password"  placeholder="ПАРОЛЬ" className="input-auth"/>
                <Button children="Войти" type="submit" disabled={!formValid}/>
            </form>
        </div>
    );
};

export default Loginbar;