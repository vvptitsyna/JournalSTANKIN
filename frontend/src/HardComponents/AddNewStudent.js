import Input from "../SimpleComponents/Input";
import React, {useEffect, useState} from "react";
import API from "../Core/api";
import Button from "../SimpleComponents/Button";
import Select from "react-select";

const AddNewStudent = ({onUserChanged, id}) => {

    const [name, setName] = useState();
    const [comment, setComment] = useState('');


    const handleSubmit = (e) => {
        e.preventDefault();
        const data = {
            subgroupWithVersionId: id,
            name: name,
            comment: comment,
        }
        API.post('Administrator/Group/addNewStudent', data, { withCredentials: true } )
            .then(res => {
                onUserChanged()
            }) .catch(res => {
        })
        console.log(data)
    }

    const handleNameChange = (e) => {
        setName(e.target.value);
    }
    const handleCommentChange = (e) => {
        setComment(e.target.value);
    }


    return (
        <div className="wrapper-add-new">
            <form onSubmit={handleSubmit} className="add-form">
                <h1>Добавить нового студента</h1>
                <Input value={name} onChange={(e) => {handleNameChange(e)}} id="name" type="text"  placeholder="Студент" className="add"/>
                <Input value={comment} onChange={(e) => {handleCommentChange(e)}} id="comment" type="text"  placeholder="Комментарий" className="add"/>
                <Button children="Создать" type="submit" disabled={!name}/>
            </form>
        </div>
    );
};

export default AddNewStudent;