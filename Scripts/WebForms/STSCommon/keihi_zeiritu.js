(function ($) {
    //消費税計算用関数
    $.fn.func_zeiritu = function (pdate,nm,val) {
        var msg_red = document.getElementById("msg_red");
        var url_wk = 'Form_SYKINDN_New.aspx/GetJOSNData_Zeiritu';
        var JSONdata = {
            //code: bumon_cd
            pdate: pdate
        };
       
        $.ajax({
            type: "POST",
            url: url_wk,
            data: JSON.stringify(JSONdata),
            datatype: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                
                var member = result.d;
                console.log(member);
                console.log(member[0].ZEIRITU);

                //結果をセット
                //wk_BUMON_NAME.innerHTML = member[0].BUMON_NM;
                nm.value = member[0].ZEIRITU;
                var intwk = parseInt(val);
                var zeikubun = document.getElementById("d_tax").value;
                //内税計算固定
                var wk = Math.floor(parseInt(member[0].ZEIRITU) * intwk / (parseInt(member[0].ZEIRITU) + 100));
                var kei = intwk - wk;
                
                var wk1 = kei.toString();
                var wk2 = wk.toString();

                var newVal_kei = wk1.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"); // (2)
                var newVal_wk =  wk2.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"); // (2)
                $('#d_syozei').val(newVal_wk);
                $('#d_keihi').val(newVal_kei);


                if (nm.innerHTML === '') {
                    msg_red.innerHTML = '';
               
                }
                else {
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

  
    return false

})(jQuery);

