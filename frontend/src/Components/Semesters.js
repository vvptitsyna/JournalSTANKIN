import React, {useEffect, useState} from "react";
import Button from "../SimpleComponents/Button";
import '../css/semesters.css'
import API from "../Core/api";
import {useDispatch, useSelector} from "react-redux";
import {getSubjectsAction, selectSemesterAction, setSemestersAction} from "../Core/semestersReducer";
import {selectSubjectAction} from "../Core/subjectsReducer";
import Slider from "react-slick";


const Semesters = ({
                   getUrl,           ...attrs
                          }) => {

    const sliderSettings = {
        className: "slider-semesters",
        dots: false,
        infinite: false,
        speed: 500,
        slidesToShow: 4,
        slidesToScroll: 1,
        prevArrow: <Button  className="custom-prev-button"/>,
        nextArrow: <Button  className="custom-next-button"/>,
    };
    const dispatch = useDispatch();
    const semesters = useSelector(state => state.sem.semesters)

    const handleGetSubjects = (id) => {
        API.get(`${getUrl}${id}`, { withCredentials: true })
            .then(res => {
                dispatch(getSubjectsAction(res.data.$values))
                dispatch(selectSemesterAction(id))
                dispatch(selectSubjectAction(res.data.$values[0].$id))
            })
    }

    return (
        <div className="semesters">
            <div className="wrapper-slider">
                <Slider {...sliderSettings}>
                    {semesters.map((semester) => {
                        return (
                            <Button onClick={() => handleGetSubjects(semester.$id)} className="semester-btn" id={semester.id} children={((semester.season===1)?"Осень":"Весна") + ' ' + semester.year} />
                        )
                    })}
                </Slider>
            </div>
        </div>
    );
};

export default Semesters;