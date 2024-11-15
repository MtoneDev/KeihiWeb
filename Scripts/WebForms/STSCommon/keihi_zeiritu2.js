(function ($) {
    //消費税計算用関数
    $.fn.func_sotozeiritu = function (pdate, nm, val) {

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
                //外税計算
                //var zei = Math.floor(parseInt(intwk * (parseInt(member[0].ZEIRITU) / 100)));
                var zei = parseInt(intwk * (parseInt(member[0].ZEIRITU) / 100));
                var wk1 = zei.toString();
                var goukei = intwk + zei;
                var newVal_gokei_wk = goukei.toString();
                var newVal_gokei = newVal_gokei_wk.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
                var newVal_zei = wk1.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"); // (2)
                var newVal_kin = val.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"); // (2)
                $('#d_syozei').val(newVal_zei);
                $('#d_keihi').val(newVal_kin);
                $('#d_gokei').val(newVal_gokei);


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

