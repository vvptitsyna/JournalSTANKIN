import Button from "../SimpleComponents/Button";
import Navigation from "../HardComponents/Navigation";
import {useNavigate} from "react-router-dom";
import ButtonGroup from "../SimpleComponents/ButtonGroup";

import '../css/administration.css'
import Footer from "../HardComponents/Footer";
function Administration () {

    const navigate = useNavigate();
    const handlerOnClick = (option) => {
        navigate(`/admin?page=${option}`)
    }
    return (
        <div className="wrapper-admin">
            <Navigation/>
            <ButtonGroup className="main-admin">
                <Button children="Управление предметами" onClick={() => handlerOnClick('subjects')}/>
                <Button children="Управление пользователями" onClick={() => handlerOnClick('users')}/>
                <Button children="Управление семестрами" onClick={() => handlerOnClick('semesters')}/>
                <Button children="Управление группами" onClick={() => handlerOnClick('groups')}/>
                <Button children="Управление связями" onClick={() => handlerOnClick('relations')}/>
                <Button children="Просмотр логов" onClick={() => handlerOnClick('logs')}/>
            </ButtonGroup>
            <Footer/>
        </div>
    );
}

export default Administration;