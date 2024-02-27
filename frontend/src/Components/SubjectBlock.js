import React from 'react'
import PropTypes from 'prop-types'
import Image from "../SimpleComponents/Image";

import '../css/subjectBlock.css'
import {useDispatch, useSelector} from "react-redux";
import {selectSubjectAction} from "../Core/subjectsReducer";
const SubjectBlock = (
    {subjectName, teacher, id, ...attrs}
) => {

    const dispatch = useDispatch();
    const subject = useSelector(state => state.subj.selectedSubject);

    const handlerSelectSubject = () => {
        dispatch(selectSubjectAction(id))
    }
    return (
        <a href="#subject" className="a-subject" onClick={() => handlerSelectSubject()}>
        <div className="subject-div">
            <Image width="70" height="70" circle="true"/>
            <div className="subject-info">
                <h1>{subjectName}</h1>
                <h2>{teacher}</h2>
            </div>
        </div>
        </a>
    );
};

SubjectBlock.propTypes = {
    subjectName: PropTypes.string,
    teacher: PropTypes.string,
}

SubjectBlock.defaultProps = {
    subjectName: "Название предмета",
    teacher: "Иванов Иван Иванович",
}
export default SubjectBlock;