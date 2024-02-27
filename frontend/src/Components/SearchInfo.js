import react, {useEffect} from 'react'

import '../css/searchInfo.css'
import Button from "../SimpleComponents/Button";
import {useDispatch, useSelector} from "react-redux";
import {useNavigate} from "react-router-dom";
const SearchInfo = () => {

    const navigate = useNavigate();
    const dispatch = useDispatch();
    const subjectId = useSelector(state => state.subj.selectedSubject)
    const subject = useSelector(state => state.sem.subjects)
    const semester = useSelector(state => state.sem.selectedSemester)

    const handlerSetSubject = () => {
        navigate('/subject')
    };


    return (
        <div className="info-block">
            <Button children="перейти" onClick={() => handlerSetSubject() }/>
        </div>
    );
};

export default SearchInfo;