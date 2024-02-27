import React from 'react'
import PropTypes from 'prop-types'
import Image from "../SimpleComponents/Image";

import '../css/subjectBlock.css'
import Button from "../SimpleComponents/Button";
import SubjectBlock from "./SubjectBlock";
import {useSelector, useStore} from "react-redux";

const SubjectInfo = () => {

    const subject = useSelector(state => state.subj.subject)
    return (
        <div className="subject-div">
            <div className="subject-info">
                <h1>Предмет:</h1>
                <h2>Лектор:{subject.lecturerName}</h2>
                {subject.teacherNames.$values.map((teacher) => {
                    return (
                        <p>Препод:{teacher}</p>
                    )
                })}
            </div>
        </div>
    );
};

SubjectInfo.propTypes = {
    subjectName: PropTypes.string,
    lector: PropTypes.string,
    teacher: PropTypes.string,
}

SubjectInfo.defaultProps = {
    subjectName: "Название предмета",
    lector: "123",
    teacher: "Иванов Иван Иванович",
}
export default SubjectInfo;