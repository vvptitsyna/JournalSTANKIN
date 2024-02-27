import '../css/file.css'
const File = ({file, onChange}) => {

    const handleFileChange = (e) => {
        onChange(e.target.files[0]);
    };

    return(
        <label className="input-file add">
            <input type="file" name="file" onChange={handleFileChange}/>
                <span className="input-file-btn">Выберите файл</span>
                <span className="input-file-text">Формат .xlsx</span>
        </label>
    );
};

export default File;