import Loginbar from "../HardComponents/Loginbar";
import Sidebar from "../HardComponents/Sidebar";
import Footer from "../HardComponents/Footer";

import '../css/general.css'
import '../css/reset.css'
import '../css/auth.css';

function Login () {
    return (
        <div className="wrapper">
            <Loginbar />
            <Sidebar />
            <Footer />
        </div>
    );
}
export default Login;