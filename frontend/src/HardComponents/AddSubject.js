import Input from "../SimpleComponents/Input";
import React, {useEffect, useState} from "react";
import API from "../Core/api";
import Button from "../SimpleComponents/Button";
import {getUsersAction} from "../Core/usersReducer";
import {useDispatch} from "react-redux";
import File from "../SimpleComponents/File";

const AddSubject = ({onUserChanged}) => {

    const [name, setName] = useState();
    const [comment, setComment] = useState('');
    const [file, setFile] = useState(null);

    const handleFileChange = (file) => {
        setFile(file);
        console.log(file);
    };

    const handleSubmitFile = (e) => {
        e.preventDefault();
        API.post("Administrator/Subject/addSubjectsFromExcel'", file, {
            withCredentials: true,
            headers: {
                "Content-Type": "multipart/form-data",
            },
        }).then((res) => {
            console.log(res.data);
            console.log(file);
            onUserChanged();
            setFile(null);
        })
            .catch((error) => {
                console.log(error);
            });
    }

    const handleSubmit = (e) => {
        e.preventDefault();

        const data = {
            name: name,
            comment: comment,
        }

        API.post('Administrator/Subject/addSubject', data, { withCredentials: true } )
            .then(res => {
                console.log (res.data);
                console.log (data);
                onUserChanged()
            }) .catch(res => {
            console.log(res.error);
            console.log (res);
        })
    }

    const nameHandler = (e) => {
        setName(e.target.value);
    }

    const commentHandler = (e) => {
        setComment(e.target.value);
    }

    return (
        <div className="wrapper-add">
            <form onSubmit={e => handleSubmit(e)} className="add-form">
                <h1>Добавить предмет</h1>
                <Input value={name} onChange={e => nameHandler(e)} id="name" type="text"  placeholder="Название предмета" className="add"/>
                <Input value={comment} onChange={e => commentHandler(e)} id="comment" type="text" placeholder="Комментарий" className="add"/>
                <Button children="Создать" type="submit" disabled={!name}/>
            </form>
            <form className="add-form" onSubmit={e =>handleSubmitFile(e)}>
                <h1>Загрузить из файла</h1>
                <File className="add" id="file" name="file" file={file} onChange={handleFileChange}/>
                <Button children="Отправить" type="submit" disabled={!file}/>
            </form>

        </div>
    );
};

export default AddSubject;