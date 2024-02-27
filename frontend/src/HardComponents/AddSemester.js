import Input from "../SimpleComponents/Input";
import React, {useEffect, useState} from "react";
import API from "../Core/api";
import Button from "../SimpleComponents/Button";
import Select from "react-select";

const AddSemester = ({onUserChanged}) => {

    const [year, setYear] = useState();
    const [season, setSeason] = useState();
    const [yearStart, setYearStart] = useState();
    const [yearEnd, setYearEnd] = useState();

    const options = [
        { value: 1, label: 'Осень' },
        { value: 2, label: 'Весна' },
    ]
    const handleSubmitAdd = (e) => {
        e.preventDefault();
        const data_add = {
            year: year,
            season: season.value,
        }
        API.post('Administrator/Semester/addSemester', data_add, { withCredentials: true } )
            .then(res => {
                onUserChanged()
            }) .catch(res => {
        })
    }

    const handleSubmitGenerate = (e) => {
        e.preventDefault();
        const data_generate = {
            yearStart: yearStart,
            yearEnd: yearEnd,
        }

        API.post('Administrator/Semester/generateSemesters', data_generate, { withCredentials: true } )
            .then(res => {
                onUserChanged()
            }) .catch(res => {
        })
    }

    const handleYearChange = (e) => {
        setYear(e.target.value);
    }
    const handleYearStartChange = (e) => {
        setYearStart(e.target.value);
    }
    const handleYearEndChange = (e) => {
        setYearEnd(e.target.value);
    }


    return (
        <div className="wrapper-add">
            <form onSubmit={handleSubmitAdd} className="add-form">
                <h1>Добавить семестр</h1>
                <Input value={year} onChange={(e) => {handleYearChange(e)}} id="year" type="text"  placeholder="Введите год семестра" className="add"/>
                <Select id="season" name="season" options={options} value={season} onChange={(season) => setSeason(season)}/>
                <Button children="Создать" type="submit" disabled={!year || !season}/>
            </form>
            <form onSubmit={handleSubmitGenerate} className="add-form">
                <h1>Добавить несколько семестров</h1>
                <Input value={yearStart} onChange={(e) => {handleYearStartChange(e)}} id="yearStart" type="text"  placeholder="С года:" className="add"/>
                <Input value={yearEnd} onChange={(e) => {handleYearEndChange(e)}} id="yearEnd" type="text"  placeholder="По год:" className="add"/>
                <Button children="Создать" type="submit" disabled={!yearStart || !yearEnd}/>
            </form>
        </div>
    );
};

export default AddSemester;