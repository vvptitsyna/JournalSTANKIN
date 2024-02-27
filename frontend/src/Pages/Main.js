import Loginbar from "../HardComponents/Loginbar";
import Sidebar from "../HardComponents/Sidebar";
import Footer from "../HardComponents/Footer";
import Navigation from "../HardComponents/Navigation";
import Semesters from "../Components/Semesters";
import Base from "../HardComponents/Base";

import '../css/main.css'
import SearchInfo from "../Components/SearchInfo";
import {useDispatch} from "react-redux";
import {useEffect} from "react";
import API from "../Core/api";
import {getSubjectsAction, selectSemesterAction, setSemestersAction} from "../Core/semestersReducer";
import {selectSubjectAction} from "../Core/subjectsReducer";
const Main= () => {

    const dispatch = useDispatch();

    useEffect(() => {
        API.get('Teacher/getTeacherSemesters', { withCredentials: true })
            .then(res => {
                dispatch(setSemestersAction(res.data.$values))
                if (res.data.$values.length > 0) {
                    dispatch(selectSemesterAction(res.data.$values[0].$id))
                }
            })

        API.get(`Teacher/getSemesterInfo?semesterId=${1}`, { withCredentials: true })
            .then(res => {
                dispatch(getSubjectsAction(res.data.$values))
                if (res.data.$values.length > 0) {
                    dispatch(selectSubjectAction(res.data.$values[0].$id))
                }

            })
    }, [dispatch])

    return (
        <div className="wrapper-main">
            <Navigation/>
            <Semesters getUrl="Teacher/getSemesterInfo?semesterId="/>
            <Base/>
            <SearchInfo/>
            <Footer/>
        </div>
    );
}

export default Main;