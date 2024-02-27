import TableHeader from "./TableHeader";
import {useDispatch, useSelector} from "react-redux";
import TableRow from "./TableRow";

import '../css/table.css'
import API from "../Core/api";
import Button from "./Button";
import React, {useState} from "react";
import {getStudentsAction, getSubjectAction, updateStudentMark} from "../Core/subjectsReducer";

const Table = () => {
    const students = useSelector(state => state.subj.students);
    const relationId = useSelector(state => state.subj.selectedRelation)
    const dispatch = useDispatch();

    const [updatedStudents, setUpdatedStudents] = useState(students);

    const handleUpdate = (studentId, fieldName, value) => {
        const updatedStudent = updatedStudents.find((student) => student.$id === studentId);
        updatedStudent[fieldName] = value;
        setUpdatedStudents([...updatedStudents]);
        console.log('id STUDENT ID IN INPUT:'+ studentId)
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        dispatch(updateStudentMark(updatedStudents));
        console.log(students)
        API.post('Teacher/saveMarks', updatedStudents, { withCredentials: true } )
            .then(res => {
                console.log (res);

            }) .catch(res => {
            console.log(res.error);
            console.log (res);
        })

        API.get(`Teacher/getRelationInfo?relationId=${relationId}`, { withCredentials: true })
            .then(res => {
                dispatch(getStudentsAction(res.data.marks.$values))
                dispatch(getSubjectAction(res.data))
            })
    };

    /*const handleSubmit = (e) => {
        e.preventDefault();

        const data = students.map(student => ({
            markId: student.markId,
            studentName: student.studentName,
            module1: student.module1,
            module2: student.module2,
            examOrTest: student.examOrTest,
            coursework: student.coursework,
        }));

        API.post('Teacher/saveMarks', data, { withCredentials: true } )
            .then(res => {
                console.log (res);

            }) .catch(res => {
            console.log(res.error);
            console.log (res);
        })
    }*/


    return (
        <div className="table">
            <TableHeader />
            <form onSubmit={handleSubmit}>
                {students.map((student) => {
                    return <TableRow key={student.$id} student={student} onUpdateStudentMark={handleUpdate}/>;
                })}
                <Button children="Отправить" type="submit" />
            </form>
        </div>
    );
};

export default Table;