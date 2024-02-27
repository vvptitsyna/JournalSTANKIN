import {useState} from "react";
import Input from "./Input";
import {useDispatch, useSelector} from "react-redux";
import {updateStudentMark} from "../Core/subjectsReducer";

const TableCell = ({
                       fieldValue, onUpdate, studentId, fieldName
                    }) => {
    const [state, setState] = useState(fieldValue);
    const dispatch = useDispatch();
    const students = useSelector(state=> state.subj.students)

    const handleChange = (e) => {
        const value = e.target.value;
        setState(value);
        console.log(students)
        onUpdate(studentId, fieldName, value);
    };
        return (
            <div className="table__cell">
                <Input value={state} onChange={e => handleChange(e)} type="text" />
            </div>
        )

}

export default TableCell;