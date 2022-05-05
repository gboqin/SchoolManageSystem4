// 校验邮箱的规则
function checkEmail(rule, value) {
    var inputName = document.getElementById(rule);
    // 验证邮箱的正则表达式
    const regEmail = /^[A-Za-z\d]+([-_.][A-Za-z\d]+)*@([A-Za-z\d]+[-.])+[A-Za-z\d]{2,4}$/;
    // 如果验证邮箱正确，返回回调函数，输入框显示绿色
    if (regEmail.test(value)) {
        inputName.style.color = "blue";

    } else {
        inputName.style.color = "red";
    }
}

function CheckEmail(value) {
    const regEmail = /^[A-Za-z\d]+([-_.][A-Za-z\d]+)*@([A-Za-z\d]+[-.])+[A-Za-z\d]{2,4}$/;
    return regEmail.test(value);
}

function CheckPassword(value) {
    return /^[A-Za-z\d-_.]/.test(value);
}



// 前后代码【略】
//<el-form-item label="密码" prop="pwd">
//    <el-input v-model="ruleForm.pwd" :type="pwdType" placeholder="请输入密码" @keyup.enter.native="login">
//    <i slot="suffix" class="el-icon-view" @click="showPwd"></i>
//  </el - input >
//</el - form - item >

//showPwd() {
//    this.pwdType === 'password' ? this.pwdType = '' : this.pwdType = 'password';
//    let e = document.getElementsByClassName('el-icon-view')[0];
//    this.pwdType == '' ? e.setAttribute('style', 'color: #409EFF') : e.setAttribute('style', 'color: #c0c4cc');
//},