import Footer from "../HardComponents/Footer";
import Navigation from "../HardComponents/Navigation";
import Semesters from "../Components/Semesters";
import Base from "../HardComponents/Base";

import '../css/main.css'
import SearchInfo from "../Components/SearchInfo";
import {useDispatch, useSelector} from "react-redux";
import {useEffect} from "react";
import API from "../Core/api";
import {getSubjectsAction, selectSemesterAction, setSemestersAction} from "../Core/semestersReducer";
import {selectSubjectAction} from "../Core/subjectsReducer";

const SupportMain = () => {

    const dispatch = useDispatch();

    useEffect(() => {
        API.get('Support/getSemesters', { withCredentials: true })
            .then(res => {
                dispatch(setSemestersAction(res.data.$values))
                if (res.data.$values.length > 0) {
                    dispatch(selectSemesterAction(res.data.$values[0].$id))
                }
            })

        API.get(`Support/getSemesterInfo?semesterId=${1}`, { withCredentials: true })
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
            <Semesters getUrl="Support/getSemesterInfo?semesterId="/>
            <Base/>
            <SearchInfo/>
            <Footer/>
        </div>
    );
}

export default SupportMain;