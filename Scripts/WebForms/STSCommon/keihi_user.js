(function ($) {
    //名表示用関数
    $.fn.func_user_nm = function (code, nm) {
        //初期表示部門名称の索引
        $('#err_msg_lbl').val('');
        var msg_red = document.getElementById("msg_red");
        var url_wk = 'Form_SYKINDN_New.aspx/SelectUserJOSNData';
        var JSONdata = {
            //code: bumon_cd
            code: code
        };
        $.ajax({
            type: "POST",
            url: url_wk,
            data: JSON.stringify(JSONdata),
            datatype: 'json',
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (result) {

                var member = result.d;
                //結果をセット
                //wk_BUMON_NAME.innerHTML = member[0].BUMON_NM;

                nm.value = member[0].USER_NAME;
                if (nm.value === '') {
                    msg_red.innerHTML = 'ユーザーIDが存在しません';
                    $('#err_msg_lbl').val('ユーザーIDが存在しません');
                    nm.value = 'ユーザーIDが存在しません';
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
                alert('異常終了！！' + errorThrown);
                return false;

            }
        }).done(function (msg) {

        });

    }

    $.fn.func_user_nm_jibumon = function (code, nm,jibumoncd) {
        //初期表示部門名称の索引
        $('#err_msg_lbl').val('');
        var msg_red = document.getElementById("msg_red");
        var url_wk = 'Form_SYKINDN_New.aspx/SelectUserJOSNData_Jibumon';
        var JSONdata = {
            //code: bumon_cd
            code: code,
            jibumoncd: jibumoncd
        };
        $.ajax({
            type: "POST",
            url: url_wk,
            data: JSON.stringify(JSONdata),
            datatype: 'json',
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (result) {

                var member = result.d;
                //結果をセット
                //wk_BUMON_NAME.innerHTML = member[0].BUMON_NM;

                nm.value = member[0].USER_NAME;
                if (nm.value === '') {
                    msg_red.innerHTML = 'ユーザーIDが不正です';
                    $('#err_msg_lbl').val('ユーザーIDが不正です');
                    nm.value = 'ユーザーIDが不正です';
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
                alert('異常終了！！' + errorThrown);
                return false;

            }
        }).done(function (msg) {

        });

    }

    $.fn.func_user_nm_nochk = function (code, nm) {
        //初期表示名称の索引
        $('#err_msg_lbl').val('');
        var msg_red = document.getElementById("msg_red");
        var url_wk = 'Form_SYKINDN_New.aspx/SelectUserJOSNData';
        var JSONdata = {
            //code: bumon_cd
            code: code
        };
        $.ajax({
            type: "POST",
            url: url_wk,
            data: JSON.stringify(JSONdata),
            datatype: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {

                var member = result.d;
                //結果をセット
                //wk_BUMON_NAME.innerHTML = member[0].BUMON_NM;
                //存在しなかったときでも表示しないだけ
                nm.value = member[0].USER_NAME;
                if (nm.value === '') {
                    msg_red.innerHTML = 'ユーザーIDが存在しません';
                    $('#err_msg_lbl').val('');
                    nm.value = '';
            
                }
                else {
                    $('#err_msg_lbl').val('');
                    msg_red.innerHTML = '';
                    $(nm).css('color', 'black');
                }

                return result;

            },
            error: function (data, errorThrown) {
                alert('異常終了！！' + errorThrown);
                return false;

            }
        }).done(function (msg) {

        });

    }


    return false



})(jQuery);

