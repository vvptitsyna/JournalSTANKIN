import Input from "../SimpleComponents/Input";
import React, {useEffect, useState} from "react";
import API from "../Core/api";
import Button from "../SimpleComponents/Button";
import Select from "react-select";

const AddStudent = ({onUserChanged, id}) => {
    const [selectedOption, setSelectedOption] = useState(null);
    const [students, setStudents] = useState([]);

    const handleSelectChange = (selectedOption) => {
        setSelectedOption(selectedOption);
    };
    const options = students.map((student) => ({
        value: student.id,
        label: student.name + ' Группа:' + student.lastSubgroup,
    }));

    useEffect(() => {
        API.get('Administrator/Group/getAllStudents', { withCredentials: true } )
            .then(res => {
                setStudents(res.data.$values)
                console.log(res.data.$values)
                console.log(students)
            }) .catch(res => {
        })
    }, [])

    const handleSubmit = (e) => {
        e.preventDefault();
        const data = {
            studentId: selectedOption.value,
            subgroupWithVersionId: id,
        }
        API.post('Administrator/Group/addExistingStudent', data, { withCredentials: true } )
            .then(res => {
                onUserChanged()
            }) .catch(res => {
        })
    }

    return (
        <div className="wrapper-add">
            <form onSubmit={handleSubmit} className="add-form">
                <h1>Добавить существующего студента</h1>
                <Select
                    id="student"
                    value={selectedOption}
                    onChange={handleSelectChange}
                    options={options}
                />
                <Button children="Создать" type="submit" disabled={!selectedOption}/>
            </form>
        </div>
    );
};

export default AddStudent;