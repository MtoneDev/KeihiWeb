(function ($) {
    //科目、内訳名表示用関数
    //税区分連動用　出金入金伝票用
    $.fn.func_kamoku_nm_zei = function (code, nm) {
        //初期表示科目名称の索引

      
        $('#d_uti_code').val('');
        $('#d_uti_name').text('');
        $('#d_tax').val('');
        var msg_red = document.getElementById("msg_red");
        var url_wk = 'Form_SYKINDN_New.aspx/SelectKamokuJOSNData';
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
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                
                var member = result.d;
                //結果をセット
                //wk_BUMON_NAME.innerHTML = member[0].BUMON_NM;
                $('#d_tax').val(member[0].TAX_CD);
           
                $('#d_keihi').css('background-color', 'lightgray');
                $('#d_syozei').css('background-color', 'lightgray');
                
                if ($('#d_tax').val() === '1') {
                 
                    $('#d_keihi').css('background-color', 'orange');
                    $('#d_syozei').css('background-color', 'orange');
                    $('input[name=z_kubun]:eq(0)').prop('checked', true);

                }

                if ($('#d_tax').val() === '2') {
                 
                    $('#d_keihi').css('background-color', 'orange');
                    $('#d_syozei').css('background-color', 'orange');
                    $('input[name=z_kubun]:eq(0)').prop('checked', true);

                }
                if ($('#d_tax').val() === '3') {
                    $('input[name=z_kubun]:eq(1)').prop('checked', true);
                }

                if($('#d_tax').val() === '4'){
                    $('input[name=z_kubun]:eq(2)').prop('checked', true);
                }

                nm.innerHTML = member[0].KAMOKU_NM;
                if (nm.innerHTML === '') {
                    msg_red.innerHTML = '科目コードが存在しません';
                    nm.innerHTML = '科目コードが存在しません';
                    $(nm).css('color', 'red');
                    $('#err_msg_lbl').val('科目コードが存在しません');
                }
                else {
                    $(nm).css('color', 'black');
                    $('#err_msg_lbl').val('');

                    msg_red.innerHTML = '';
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
    //科目、内訳名表示用関数
    //税区分連動用　請求伝票伝票用
    $.fn.func_kamoku_nm_zei2 = function (code, nm) {
        //初期表示科目名称の索引


        $('#d_uti_code').val('');
        $('#d_uti_name').text('');
        $('#d_tax').val('');
        var msg_red = document.getElementById("msg_red");
        var url_wk = 'Form_SYKINDN_New.aspx/SelectKamokuJOSNData';
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
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (result) {

                var member = result.d;
                //結果をセット
                //wk_BUMON_NAME.innerHTML = member[0].BUMON_NM;
                $('#d_tax').val(member[0].TAX_CD);

                $('#d_keihi').css('background-color', 'lightgray');
                $('#d_syozei').css('background-color', 'lightgray');

                if ($('#d_tax').val() === '1') {

                    $('#d_keihi').css('background-color', 'orange');
                    $('#d_syozei').css('background-color', 'orange');
                    $('input[name=z_kubun]:eq(0)').prop('checked', true);

                }

                if ($('#d_tax').val() === '2') {

                    $('#d_keihi').css('background-color', 'orange');
                    $('#d_syozei').css('background-color', 'orange');
                    $('input[name=z_kubun]:eq(1)').prop('checked', true);

                }
                if ($('#d_tax').val() === '3') {
                    $('input[name=z_kubun]:eq(2)').prop('checked', true);
                }

                if ($('#d_tax').val() === '4') {
                    $('input[name=z_kubun]:eq(3)').prop('checked', true);
                }

                nm.innerHTML = member[0].KAMOKU_NM;
                if (nm.innerHTML === '') {
                    msg_red.innerHTML = '科目コードが存在しません';
                    nm.innerHTML = '科目コードが存在しません';
                    $(nm).css('color', 'red');
                    $('#err_msg_lbl').val('科目コードが存在しません');
                }
                else {
                    $(nm).css('color', 'black');
                    $('#err_msg_lbl').val('');

                    msg_red.innerHTML = '';
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

    //課税名称のみ
    $.fn.func_kamoku_nm = function (code, nm) {
        //初期表示科目名称の索引
        

        var msg_red = document.getElementById("msg_red");
        var url_wk = 'Form_SYKINDN_New.aspx/SelectKamokuJOSNData';
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


                nm.innerHTML = member[0].KAMOKU_NM;
                if (nm.innerHTML === '') {
                    msg_red.innerHTML = '科目コードが存在しません';
                    nm.innerHTML = '科目コードが存在しません';
                    $(nm).css('color', 'red');
                    $('#err_msg_lbl').val('科目コードが存在しません');
                }
                else {
                    $(nm).css('color', 'black');
                    $('#err_msg_lbl').val('');
                    msg_red.innerHTML = '';
                    $('#err_msg_lbl').val('')
                }

                return result;

            },
            error: function (data, errorThrown) {
                alert('異常終了！！' + errorThrown);
                console.log(errorThrown);
            }
        }).done(function (msg) {

        });

    }

    $.fn.func_utiwake_nm = function (kamoku,utiwake, nm) {
        //初期表示内訳名称の索引

        var msg_red = document.getElementById("msg_red");
        var url_wk = 'Form_SYKINDN_New.aspx/SelectUtiwakeJOSNData';
        var JSONdata = {
            kamoku: kamoku,
            utiwake: utiwake
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
                nm.innerHTML = member[0].UCHI_NM;
            
                if (nm.innerHTML === '') {
                    msg_red.innerHTML = '内訳コードが存在しません';
                    nm.innerHTML = '内訳コードが存在しません';
                    $('#err_msg_lbl').val('内訳コードが存在しません');
                    $(nm).css('color', 'red');
                }
                else {
                    $('#err_msg_lbl').val('');
                    $(nm).css('color', 'black');
                    msg_red.innerHTML = '';
                }

                return result;

            },
            error: function (data, errorThrown) {
                alert('異常終了！！' + errorThrown);
            }
        }).done(function (msg) {

        });

    }

    $.fn.func_utiwake_chk = function (utiwake, nm) {
        //初期表示内訳名称の索引
        var msg_red = document.getElementById("msg_red");
        var url_wk = 'Form_SYKINDN_New.aspx/GetJOSNData_Utiwake';
        var JSONdata = {
            code: utiwake
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
                //存在しないとき               
                if (member[0].UCHI_NM === '') {
                    $('#d_uti_code').prop('disabled', true);
                    $('#uti_serach').prop('disabled', true);
                    $('#d_tekiyo').focus();
                }
                //存在するとき              
                else {
                    $(nm).css('color', 'black');
                    msg_red.innerHTML = '';
                    $('#d_uti_code').focus();
                }
               // nm.innerHTML = '';
                return result;
            },
            error: function (data, errorThrown) {
                alert('異常終了！！' + errorThrown);
                console.log(errorThrown);
            }
        }).done(function (msg) {

        });

    }

    return false

})(jQuery);
