(function ($) {
    //部門名表示用関数
    $.fn.func_bumon_nm =function(code,nm) {
        //初期表示部門名称の索引
       
        console.log(code);
        var msg_red = document.getElementById("msg_red");
        var url_wk = 'Form_M_USER.aspx/SelectBumonJOSNData';
        var JSONdata = {
            //code: bumon_cd
            code: code
        };
        $.ajax({
            type: "POST",
            url: url_wk,
            data: JSON.stringify(JSONdata),
            datatype: 'json',
            timeout: 5000,
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                var member = result.d;
                //結果をセット
                //wk_BUMON_NAME.innerHTML = member[0].BUMON_NM;
                 nm.innerHTML = member[0].BUMON_NM;
                if (nm.innerHTML === '') {
                    msg_red.innerHTML = '部門コードが存在しません';
                    $('#err_msg_lbl').val('部門コードが存在しません');

                    nm.innerHTML = '部門コードが存在しません';
                    $(nm).css('color', 'red');
                 }
                else {
                    $('#err_msg_lbl').val('');
                    msg_red.innerHTML = '';
                    $(nm).css('color', 'black');
                }

                return result;

            },
            error: function (data, errorThrown) {
                console.log(errorThrown);
            }
        }).done(function (msg) {

        });
    
    }
    return false

})(jQuery);






