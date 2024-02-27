import React from "react";

import '../css/base.css'
import SubjectBlock from "../Components/SubjectBlock";
import {useSelector} from "react-redux";
import Button from "../SimpleComponents/Button";


const Base = () => {

    const subjects = useSelector(state => state.sem.subjects)

    return (
        <div className="common-main">
            {subjects.map((subject) => {
                return (
                    <SubjectBlock subjectName = {subject.subjectName} teacher={subject.lecturerName} id={subject.subjectId} />
                )
            })}
            <SubjectBlock />

        </div>
    );
};

export default Base;