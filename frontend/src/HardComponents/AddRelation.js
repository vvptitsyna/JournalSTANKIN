import Input from "../SimpleComponents/Input";
import React, {useEffect, useState} from "react";
import API from "../Core/api";
import Button from "../SimpleComponents/Button";
import Select from "react-select";
import {getAdminGroupsAction, getAdminSubGroupsAction, selectedAdminGroupAction} from "../Core/adminGroupsReducer";
import {useDispatch, useSelector} from "react-redux";
import {getAdminRelationsAction, getAdminTeachersAction} from "../Core/adminRelationsReducer";
import {getAdminSubjectsAction} from "../Core/adminSubjectsReducer";
import {getUsersAction} from "../Core/usersReducer";
import {getAdminSemestersAction} from "../Core/adminSemestersReducer";

const AddRelation = ({onUserChanged}) => {

    const dispatch=useDispatch();
    const relations = useSelector(store => store.adminRelations.relations)
    const [lectorName, setLectorName] = useState();
    const [teachers, setTeachers] = useState(useSelector(store => store.adminRelations.teachers));
    const [subjects, setSubjects] = useState(useSelector(store => store.adminSubjects.subjects));
    const [groups, setGroups] = useState(useSelector(store => store.adminGroups.groups));
    const [subgroups,setSubgroups] = useState([]);
    const [semesters, setSemesters] = useState(useSelector(store => store.adminSemesters.semesters));

    const [selectedLector, setSelectedLector] = useState(null);
    const [selectedTeachers, setSelectedTeachers] = useState(null);
    const [selectedSubject, setSelectedSubject] = useState(null);
    const [selectedGroup, setSelectedGroup] = useState(null);
    const [selectedSubgroups, setSelectedSubgroups] = useState([]);
    const [selectedSemester, setSelectedSemester] = useState(null);
    const [selectedSubjectForm, setSelectedSubjectForm] = useState(null);
    const [hasCoursework, setHasCoursework] = useState(false);

    useEffect(() => {
        API.get(`Administrator/Relation/getRelations`, { withCredentials: true })
            .then(res => {
                dispatch(getAdminRelationsAction(res.data.$values))
                console.log(res.data.$values)
            }).catch(res => {
            console.log(res.error);
        })

        API.get(`Administrator/Relation/getTeachers`, { withCredentials: true })
            .then(res => {
                dispatch(getAdminTeachersAction(res.data.$values))
                console.log(res.data.$values)
            }).catch(res => {
            console.log(res.error);
        })

        API.get(`Administrator/Subject/getSubjects`, { withCredentials: true })
            .then(res => {
                dispatch(getAdminSubjectsAction(res.data.$values))
            }).catch(res => {
            console.log(res.error);
        })
        API.get(`Administrator/Relation/getRelations`, { withCredentials: true })
            .then(res => {
                dispatch(getAdminRelationsAction(res.data.$values))
                console.log(res.data.$values)
            }).catch(res => {
            console.log(res.error);
        })
        API.get(`Administrator/Group/getGroups`, { withCredentials: true })
            .then(res => {
                dispatch(getAdminGroupsAction(res.data.$values))
            }).catch(res => {
            console.log(res.error);
        })
        API.get(`/Administrator/Semester/getSemesters`, { withCredentials: true })
            .then(res => {
                dispatch(getAdminSemestersAction(res.data.$values))
                console.log(semesters)
            }).catch(res => {
            console.log(res.error);
        })
    }, []);

    const optionsLector = teachers.map((teacher) => ({
        value: teacher.id,
        label: teacher.userName,
    }));

    const optionsSubject = subjects.map((subject) => ({
        value: subject.id,
        label: subject.name,
    }));

    const optionsGroups = groups.map((group) => ({
        value: group.id,
        label: group.name,
    }));
    const optionsSubGroups = subgroups.map((subgroup) => ({
        value: subgroup,
        label: subgroup,
    }));
    const optionsSemesters = semesters.map((semester) => ({
        value: semester.id,
        label: ((semester.season===1)?"Осень":"Весна") + ' ' + semester.year,
    }));

    const optionsExamOrTest = [
        { value: 1, label: 'Зачет' },
        { value: 2, label: 'Экзамен' },
    ]
    const handleSubmitAdd = (e) => {
        e.preventDefault();
        const relationData = {
            lecturerId: selectedLector.value,
            teachersId: selectedTeachers.map((option) => option.value),
            subjectId: selectedSubject.value,
            groupId: selectedGroup.value ,
            subgroupName: selectedSubgroups.value,
            semesterId: selectedSemester.value,
            subjectForm: selectedSubjectForm.value,
            hasCoursework: hasCoursework,
        }
        console.log(relationData)
        API.post('Administrator/Relation/addRelation', relationData, { withCredentials: true } )
            .then(res => {
                onUserChanged()
                console.log(res)
                console.log("ВСЕ СУПЕР")
            }) .catch(res => {
                console.log(res)
                console.log("ВСЕ ПЛОХО")
        })
    }

    const handlerLector = (selectedLector) => {
        setSelectedLector(selectedLector)
    }
    const handlerTeachers = (selectedTeachers) => {
        setSelectedTeachers(selectedTeachers)
    }
    const handlerSubject = (selectedSubject) => {
        setSelectedSubject(selectedSubject)
    }
    const handlerSubgroup = (selectedSubgroups) => {
        setSelectedSubgroups(selectedSubgroups)
    }
    const handlerSemester = (selectedSemester) => {
        setSelectedSemester(selectedSemester)
    }
    const handlerExamOrTest = (selectedValue) => {
        setSelectedSubjectForm(selectedValue)
    }

    const handleHasCourseworkChange = (event) => {
        setHasCoursework(event.target.checked);
    }

    const handlerGroup = (selectedGroup) => {
        setSelectedGroup(selectedGroup)
        document.getElementById("subgroups").selectedIndex = -1;
        API.get(`Administrator/Relation/getSubgroups?mainGroupId=${selectedGroup.value}`, { withCredentials: true })
            .then(res => {
                dispatch(getAdminSubGroupsAction(res.data.$values))
                setSubgroups(res.data.$values)
                console.log(subgroups)
            }).catch(res => {
            console.log(res.error);
        })
    }


    return (
        <div className="wrapper-add">
            <form onSubmit={e => handleSubmitAdd(e)} className="">
                <h1>Добавить связь</h1>
                <Select name="lecturerId" value={selectedLector} onChange={handlerLector} options={optionsLector} placeholder="Выберите лектора"/>
                <Select name="teachersId" value={selectedTeachers} onChange={handlerTeachers} options={optionsLector} placeholder="Выберите преподавателей" isMulti={true}/>
                <Select name="subjectId" value={selectedSubject} onChange={handlerSubject} options={optionsSubject} placeholder="Выберите предмет"/>
                <Select name="groupId" value={selectedGroup} onChange={handlerGroup} options={optionsGroups} placeholder="Выберите группу"/>
                <Select name="subgroupName" value={selectedSubgroups} onChange={handlerSubgroup} options={optionsSubGroups} placeholder="Выберите подгруппу" id="subgroups"/>
                <Select name="semesterId" value={selectedSemester} onChange={handlerSemester} options={optionsSemesters} placeholder="Выберите семестр"/>
                <Select name="subjectForm" value={selectedSubjectForm} onChange={handlerExamOrTest} options={optionsExamOrTest} placeholder="Зачет/экзамен"/>
                <Input name="hasCoursework" onChange={handleHasCourseworkChange} type="checkbox" label="Курсовая работа"/>
                <Button children="Создать" type="submit" />
            </form>
        </div>
    );
};

export default AddRelation;