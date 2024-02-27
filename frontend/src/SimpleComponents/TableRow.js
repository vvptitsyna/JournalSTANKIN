import TableCell from "./TableCell";
import PropTypes from "prop-types";
import Input from "./Input";

const TableRow = ({ student, onUpdateStudentMark }) => {
    return (
        <div className="table__row">
            <TableCell
                studentId={student.$id}
                fieldName="studentName"
                fieldValue={student.studentName}
                onUpdate={onUpdateStudentMark}
            />
            <TableCell
                studentId={student.$id}
                fieldName="module1"
                fieldValue={student.module1}
                onUpdate={onUpdateStudentMark}
            />
            <TableCell
                studentId={student.$id}
                fieldName="module2"
                fieldValue={student.module2}
                onUpdate={onUpdateStudentMark}
            />
            <TableCell
                studentId={student.$id}
                fieldName="examOrTest"
                fieldValue={student.examOrTest}
                onUpdate={onUpdateStudentMark}
            />
            <TableCell
                studentId={student.$id}
                fieldName="coursework"
                fieldValue={student.coursework}
                onUpdate={onUpdateStudentMark}
            />
        </div>
    );
};

export default TableRow;