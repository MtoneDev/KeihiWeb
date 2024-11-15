(function ($) {
    //名表示用関数
    $.fn.func_trade_nm = function (code, nm, ad) {
        //初期表示部門名称の索引
        $('#err_msg_lbl').val('');
       
        var msg_red = document.getElementById("msg_red");
        var url_wk = 'Form_SEIKYUDN_New.aspx/SelectTradeJOSNData';
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
                nm.value = member[0].TRADE_NM;
                ad.value = member[0].ADDRESS1;
                if (nm.value === '') {
                    msg_red.innerHTML = '取引コードが存在しません';
                    $('#err_msg_lbl').val('取引コードが存在しません');
                    nm.value = '取引コードが存在しません';
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
    return false

})(jQuery);


