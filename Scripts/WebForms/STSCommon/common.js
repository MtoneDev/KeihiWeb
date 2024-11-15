//数字キーの入力のみ許可
function CheckNumNumelic() {

    //alert(event.keyCode);
    if (((event.keyCode < 48) || (event.keyCode > 57))) {
        if (event.keyCode != 13) {
            //window.event.returnValue = false;
            return false;
        }
    }
}

//合計欄の計算（フッタ欄）
function syukei_calc(keihi_col, zei_col, kin_col) {
    var tr = $("#MainContent_GridView1 tr");
    //合計欄再計算
    var goukei_keihi = 0;
    var goukei_zei = 0;
    var goukei_kin = 0;
    for (var i = 1, l = tr.length ; i < l; i++) {
        var cells = tr.eq(i).children();//1行目から順にth、td問わず列を取得
        var wk = cells.eq(1).text();//

        var keihi_kanma = cells.eq(keihi_col).text();
        var keihi = keihi_kanma.replace(/,/g, ""); //カンマカット
        var zei_kanma = cells.eq(zei_col).text();
        var zei = zei_kanma.replace(/,/g, ""); //カンマカット
        var kin_kanma = cells.eq(kin_col).text();
        var kin = kin_kanma.replace(/,/g, ""); //カンマカット
        var pattern = /^[-]?([1-9]\d*|0)(\.\d+)?$/;

        if (wk === ' ') {
        } else {
            // 数値チェック
            if (pattern.test(parseInt(keihi))) {

                goukei_keihi = parseInt(keihi) + parseInt(goukei_keihi);

            }

            if (pattern.test(parseInt(zei))) {

                goukei_zei = parseInt(zei) + parseInt(goukei_zei);

            }
            if (pattern.test(parseInt(kin))) {

                goukei_kin = parseInt(kin) + parseInt(goukei_kin);
            }

        }

    }

    var goukei_keihi_string = goukei_keihi.toString();
    var goukei_keihi_kanma = goukei_keihi_string.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"); // (2)
    var goukei_zei_string = goukei_zei.toString();
    var goukei_zei_kanma = goukei_zei_string.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"); // (2)

    var goukei_kin_string = goukei_kin.toString();
    var goukei_kin_kanma = goukei_kin_string.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"); // (2)


    $('#goukeiran_keihi').text(goukei_keihi_kanma);
    $('#goukeiran_zei').text(goukei_zei_kanma);
    $('#goukeiran_kin').text(goukei_kin_kanma);

};

//金額カンマ編集
function kanma_func(obj) {
    var wk = obj.val();

    if (wk.length > 0) {

        //金額カンマ編集
        var i = wk.split('.');
        var val = i[0].replace(/,/g, "");                  // (1)
        var newVal = val.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"); // (2)
        obj.val(newVal);

    }

};

//空白変換　&nbsp　⇒ ''
function nbsp_func(valwk) {

    if (valwk.length===1 && valwk.charCodeAt(0) === 160) {
        return '';
    } else {
        return valwk;
    }
        
};

//金額編集、消費税計算用の関数
function calc_kingaku(obj,datewk) {

    if ($('#d_kingaku').val() > '') {
        //消費税計算
        //var pdate = $('#<%=kounyu_date.ClientID%>').val();
        var pdate = datewk;

        var nm = document.getElementById('d_zeiritu');
        var kin_val_wk = document.getElementById('d_kingaku').value;
        var kin_val = kin_val_wk.replace(/,/g, "");

        if ($('input[name="z_kubun"]:eq(0)').is(':checked')) {

            $('#d_zeiritu').func_zeiritu(pdate, nm, kin_val);
        }
      
        //金額カンマ編集
        var val = obj.val().replace(/,/g, "");                  // (1)
        var newVal = val.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"); // (2)
        obj.val(newVal);
        if ($('input[name="z_kubun"]:eq(0)').is(':checked')) {
        } else {
            $('#d_keihi').val(newVal);
            $('#d_syozei').val('0');
        }

        $('#d_gokei').val(newVal);
    }

};
//金額編集、消費税計算用の関数
//請求伝票用
function calc_kingaku2(obj, datewk) {

    if ($('#d_kingaku').val() > '') {
        //消費税計算
        //var pdate = $('#<%=kounyu_date.ClientID%>').val();
        var pdate = datewk;

        var nm = document.getElementById('d_zeiritu');
        var kin_val_wk = document.getElementById('d_kingaku').value;
        var kin_val = kin_val_wk.replace(/,/g, "");

        if ($('input[name="z_kubun"]:eq(0)').is(':checked')) {

            $('#d_zeiritu').func_zeiritu(pdate, nm, kin_val);
        }
        if ($('input[name="z_kubun"]:eq(1)').is(':checked')) {

            $('#d_zeiritu').func_sotozeiritu(pdate, nm, kin_val);
        }

        //金額カンマ編集
        var val = obj.val().replace(/,/g, "");                  // (1)
        var newVal = val.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"); // (2)
        obj.val(newVal);
        if ($('input[name="z_kubun"]:eq(0)').is(':checked')) {
            $('#d_gokei').val(newVal);
        } else {
            if ($('input[name="z_kubun"]:eq(1)').is(':checked')) {
            } else {
                $('#d_keihi').val(newVal);
                $('#d_syozei').val('0');
                $('#d_gokei').val(newVal);
            }
        }
        //$('#d_gokei').val(newVal);
      

    }

};







