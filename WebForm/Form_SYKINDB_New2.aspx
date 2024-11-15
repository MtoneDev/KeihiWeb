<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Form_SYKINDB_New2.aspx.vb" Inherits="KeihiWeb.Form_SYKINDB_New2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<style type="text/css">
   table#user tr:hover {
    background-color: #CC99FF; /* 行の背景色 */
    }
   table#kamoku tr:hover {
    background-color: #CC99FF; /* 行の背景色 */
    }
    table#utiwake tr:hover {
    background-color: #CC99FF; /* 行の背景色 */
    }
 
     td.nowrap {
      white-space: nowrap;
    }

   .header0{
          position:relative;
          }

   .denban{
          position:absolute;
          top:10px;
          background-color:yellow;
          left:70px;
          }
   .zandaka{
       position:absolute;
       top:10px;
       right:0px;
       background-color:orange;
   }
   .denban_lbl{
          position:absolute;
          top:10px;
          left:0px;
          }
   .zandaka_lbl{
       position:absolute;
       top:10px;
       right:150px;
   }
   .bottun_close {
       position:absolute;
       right:45px;
   }
   .header_group1{  
    top:100px; 
    }
   .panel{
       position:absolute;
       top:70px;
   }
   .meisai{
          position:relative;
          top:150px;
   }
        
</style>
<script type="text/javascript" src="../Scripts/keihi_user.js" ></script>
<script type="text/javascript" src="../Scripts/keihi_bumon.js" ></script>
<script type="text/javascript" src="../Scripts/keihi_kamoku.js" ></script>
<script type="text/javascript" src="../Scripts/keihi_zeiritu.js" ></script>
<style type="text/css">
     /* --- マウスオーバー時 --- */
table#bumon tr:hover {
background-color: #CC99FF; /* 行の背景色 */
}
</style>

<script type="text/javascript">

$(function() {

    //金額編集、消費税計算用の関数
    function calc_kingaku(obj) {
       
        if ($('#d_kingaku').val() > '') {
                //消費税計算
                var pdate = $('#<%=kounyu_date.ClientID%>').val();
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
  　///**********　　　
    //初期セット
    ///**********　　　
    //datepiclker
    $.datepicker.setDefaults($.datepicker.regional['ja']);
    $('#<%=kounyu_date.ClientID%>').datepicker({
      dateFormat: 'yy/mm/dd',
      showOn: 'both'
    });
    var wk_GYO = document.getElementById("d_gyo");
    wk_GYO.value = '';

    //伝票入力画面にての部門ｺｰﾄﾞ、部門名の表示
    var auth_bumon_code = document.getElementById("auth_bumon_cd");
    var bumon_nm = document.getElementById("d_bumon_name");
    $('#d_bumon_id').val(auth_bumon_code.value);
    $('#d_bumon_name').func_bumon_nm(auth_bumon_code.value, bumon_nm);
    //更新ボタンを有効
    $('#<%=button_kousin.ClientID%>').prop('disabled', true);
      

    //税金のラジオボタン初期値セット
    $('input[name=z_kubun]:eq(1)').prop('checked', true);
    //消費税などの入力制御
    $('#d_keihi').prop('readonly', true);
    $('#d_keihi').css('background-color', 'lightgray');
    $('#d_syozei').prop('readonly', true);
    $('#d_syozei').css('background-color', 'lightgray');
    $('#d_gokei').prop('readonly', true);
    $('#d_gokei').css('background-color', 'lightgray');

    //内訳選択画面
    var di_utiwake = $('#dialog_utiwake');
    di_utiwake.dialog({
        modal: true,
        width: 400,
        height: 400,
        cache: false,
        autoOpen: false,
        buttons: {
            '閉じる': function () {
                $(this).dialog('close');
            }
        }
    });

    //科目選択画面
    var di_kamoku = $('#dialog_kamoku');
    di_kamoku.dialog({
        modal: true,
        width: 400,
        height: 400,
        autoOpen: false,
        buttons: {
            '閉じる': function () {
                $(this).dialog('close');
            }
        }
    });


    //部門選択画面
    var di_bumon = $('#dialog_bumon');
    di_bumon.dialog({
        modal: true,
        width: 500,
        height: 400,
        autoOpen: false,
        buttons: {
            '閉じる': function () {
                $(this).dialog('close');
            }
        }
    });


    //ユーザー選択画面
    var di_user =$('#dialog_user');
    di_user.dialog({
        modal: true,
        width: 500,
        height: 400,
        autoOpen: false,
        buttons:{ 
            '閉じる': function () {
                $(this).dialog('close');
            }
        }
    });

    //メッセージ用ダイアログ
    var di_msg = $('#dialog_msg');
    //ダイアログを初期化（自動オープンしない）
    di_msg.dialog({

        modal: false,
        width: 250,
        height: 170,
        autoOpen: false,
        buttons: {

            '閉じる': function () {
                var msg_status = document.getElementById("msg_status");

                //window.location.reload();
                $(this).dialog('close');
                if (msg_status.value === 'OK') {
                    di.dialog('close');
                }
            }
        }
    });

    $('#user').click(function () {
        di_user.dialog('close');

    });
    $('#bumon').click(function () {
        di_bumon.dialog('close');
        $('#d_shiharai').focus();

    });
    $('#kamoku').click(function () {
        di_kamoku.dialog('close');
        $('#d_kamoku_code').focus();
        $('#d_uti_code').focus();

    });

    $('#utiwake').click(function () {
        di_utiwake.dialog('close');
        $('#d_tekiyo').focus();
    });


    //*****明細登録画面******
    var di = $('#dialog_syukin');
    //ダイアログを初期化（自動オープンしない）
    di.dialog({
        modal: true,
        width: 600,
        height: 640,
        autoOpen: false,
        buttons: {

            '更新': function () {
                var bumon_cd = document.getElementById("d_bumon_id").value;
                var bumon_nm = document.getElementById("d_bumon_name").innerHTML;
                var shiharai = document.getElementById("d_shiharai").value;
                var kamoku_cd = document.getElementById("d_kan_code").value;
                var kamoku_nm = document.getElementById("d_kan_name").innerHTML;
                var uchi_cd = document.getElementById("d_uti_code").value;
                var uchi_nm = document.getElementById("d_uti_name").innerHTML ;
                var tekiyo = document.getElementById("d_tekiyo").value;
                var keihi = document.getElementById("d_keihi").value;
                var zei = document.getElementById("d_syozei").value;
                var kin = document.getElementById("d_kingaku").value;
                var wk;
                var rec = 0;
                var gyo = 0;

                var select_GYO = document.getElementById("d_gyo");
              
                //sinnki
                //gridviewの登録レコード数を取得

                //select_GYOが空で新規登録
                if (select_GYO.value === '') {
                    var tr = $("#highlight_after tr");
                    for (var i = 1, l = tr.length - 1; i < l; i++) {
                        var cells = tr.eq(i).children();//1行目から順にth、td問わず列を取得
                        var wk = cells.eq(1).text();//
                        if (wk === '') {
                        } else {
                            rec = parseInt(rec) + 1;
                        }

                    }
                    rec = parseInt(rec) + 1;
                } else {
                    //select_GYOが空以外で修正登録
                    rec = parseInt(select_GYO.value);
                }
                var tr = $("#highlight_after tr");
                //var tr = $("#MainContent_GridView1 tr");
                //var table = document.getElementById("table_meisai");
               // var col = document.createElement('col');
                //table.rows[rec].cells[1].innerHTML = bumon_cd;
                <%--                var dd = document.getElementById("<%=tID%>'MainContent_lblBUMON_CD");
                dd.innerHTML(bumon_cd);--%>
                var ddd = document.getElementById("bumon_cd_s");

                var cells = tr.eq(rec).children();
                cells.eq(1).find("input").val(bumon_cd);
                cells.eq(2).text(shiharai);
                cells.eq(3).text(kamoku_cd);
                cells.eq(4).text(kamoku_nm);
                cells.eq(5).text(uchi_cd);
                cells.eq(6).text(uchi_nm);
                cells.eq(7).text(tekiyo);
                cells.eq(8).text(keihi);
                cells.eq(9).text(zei);
                cells.eq(10).text(kin);
              

                //合計欄再計算
                var goukei_keihi = 0;
                var goukei_zei = 0;
                var goukei_kin = 0;
                for (var i = 1, l = tr.length - 1; i < l; i++) {
                    var cells = tr.eq(i).children();//1行目から順にth、td問わず列を取得
                    var wk = cells.eq(1).text();//

                    var keihi_kanma = cells.eq(8).text();
                    var keihi = keihi_kanma.replace(/,/g, ""); //カンマカット
                    
                    var zei_kanma = cells.eq(9).text();
                    var zei = zei_kanma.replace(/,/g, ""); //カンマカット

                    var kin_kanma = cells.eq(10).text();
                    var kin = kin_kanma.replace(/,/g, ""); //カンマカット
                   
                    if (wk===' ') {
                    } else {
                        
                        goukei_keihi = parseInt(keihi) + parseInt(goukei_keihi);
                        goukei_zei = parseInt(zei) + parseInt(goukei_zei);
                        goukei_kin = parseInt(kin) + parseInt(goukei_kin);
                   
                    }

                }

                var goukei_keihi_string = goukei_keihi.toString();
                var goukei_keihi_kanma = goukei_keihi_string.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"); // (2)
                var goukei_zei_string = goukei_zei.toString();
                var goukei_zei_kanma = goukei_zei_string.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"); // (2)
                
                var goukei_kin_string = goukei_kin.toString();
                var goukei_kin_kanma = goukei_kin_string.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"); // (2)

                var cells = tr.eq(11).children();//footer行目から順にth、td問わず列を取得
                
                cells.eq(8).text(goukei_keihi_kanma);
                cells.eq(9).text(goukei_zei_kanma);
                cells.eq(10).text(goukei_kin_kanma);
            

                var msg_red = document.getElementById("msg_red").innerHTML;

                $(this).dialog('close');

            },

            '閉じる': function () {
                $(this).dialog('close');
            }
        }
    });
    //******
    //****行追加ボタン
    //******
    $('#<%=gyo_new.ClientID%>').click(function () {
            //明細登録画面

            var wk_GYO = document.getElementById("d_gyo");
            wk_GYO.value = '';

            di.dialog('open');
            $('#d_bumon_id').val(auth_bumon_code.value);
            $('#d_bumon_name').func_bumon_nm(auth_bumon_code.value, bumon_nm);

            $('#d_keihi').css('background-color', 'lightgray');
            $('#d_syozei').css('background-color', 'lightgray');
            $('input[name=z_kubun]:eq(1)').prop('checked', true);
            $('#d_shiharai').focus();
            $('#d_shiharai').val('');
            $('#d_kan_code').val('');
            $('#d_kan_name').text('');
            $('#d_uti_code').val('');
            $('#d_uti_name').text('');
            $('#d_tekiyo').val('');
            $('#d_tax').val('');
            $('#d_kingaku').val('');
           //消費税などの入力制御
            $('#d_keihi').val('')
            $('#d_syozei').val('');
            $('#d_gokei').val('');
         
            return false;
        });

        // 編集ボタンのイベントハンドラ
        $("#edituser").click(function () {

            var wk_USER_ID = document.getElementById("d_user_id").value;
            if (wk_USER_ID.length === 0) {
                var msg = document.getElementById("msg");
                msg.innerHTML = '行選択してください';
                di_msg.dialog('open');
                return false
            }
            //編集画面オープン
            di.dialog('open');

            return false;
        });

    //*****
    //****確認ボタン
    //*****
       // $('#<%=button_kousin.ClientID%>').click(function () {
       $('#button_kakunin').click(function () {
           //***
           //伝票番号の取得 
            //キー生成（自部門コードと購入日YYMM)
            //***
           var msg_red = document.getElementById("msg_red");
           var den_no = $('#<%=denban1.ClientID%>');

           var jibumoncd = document.getElementById("auth_bumon_cd").value;
           var koubay_date = $('#<%=kounyu_date.clientID%>').val();
           var koubay_date_val = koubay_date.replace(/\u002f/g, '');
      
           var YM = koubay_date_val.substring(4, 8);
           var inputym = YM;

           var url_wk = 'Form_SYKINDN_New.aspx/GetJOSNDenpyo_NO';
           var JSONdata = {
               //code: bumon_cd
               jibumoncd: jibumoncd,
               ym:inputym
           };
            //伝票番号の取得 
           $.ajax({
               type: "POST",
               url: url_wk,
               data: JSON.stringify(JSONdata),
               datatype: 'json',
               timeout: 5000,
               contentType: 'application/json; charset=utf-8',
               success: function (result) {
                   var member = result.d;
                   console.log(member);
                   var no = parseInt(member[0].MAXSLIP_NO) + 1;
                   //wk_BUMON_NAME.innerHTML = member[0].BUMON_NM;
                   den_no.val(no);

                   //var a = document.getElementById("bumon_cd01").innerHTML;

                   var div_element = document.getElementById('hidden');
                   //動的に生成する
                   var tr = $('#table_meisai tr');
                   for (var i = 1, l = tr.length - 1; i < l; i++) {
                       var cells = tr.eq(i).children();//1行目から順にth、td問わず列を取得
                       var wk = cells.eq(1).text();//

                       if (wk === '') {
                       } else {
                           alert(wk);

                           var namewk = 'h_bumon_cd' + i;
                           div_element.innerHTML = '<input type="hidden" name="' + namewk + '"' + 'id="' + namewk + '">';
                           alert(div_element.innerHTML);
                           document.getElementById(namewk).value = wk;
                           alert(namewk);
                       }

                   }
                   return result;

               },
               error: function (data, errorThrown) {
                   console.log(errorThrown);
               }
           }).done(function (msg) {

           });

           //更新ボタンを有効
            $('#<%=button_kousin.ClientID%>').prop('disabled', false);
            return true;
        });


    //ユーザーチェンジイベント
    //ユーザー名表示
    $('#d_user_cd').on({
        change: function () {
            var msg_red = document.getElementById("msg_red");
            msg_red.innerHTML = '';

            var user_cd = document.getElementById("d_user_cd").value;

            var wk_USER_NAME = document.getElementById("d_user_name");
            if (user_cd.length > 0) {
                //ユーザー表示用関数
                $('#d_user_name').func_user_nm(user_cd, wk_USER_NAME);
            } else {
                $('#d_user_name').val('');
            }
        }
    });
    //ユーザー選択画面オープン
    $('#user_serach').click(function (e) {

        var msg_red = document.getElementById("msg_red");
        msg_red.innerHTML = '';

        $.ajax({
            type: "POST",
            url: "Form_SYKINDN_New.aspx/GetJOSNData_User",
            contentType: "application/json;charset=utf-8",
            data: {},
            dataType: "json",
            success: function (data) {

                if (data.d.length > 0) {
                    $('#user').append("<tr style='background-color:#0857E0'><th>USER_CD</th><th>USER_NAME</th><th>BUMON_CD</th></tr>");

                    for (var i = 0; i < data.d.length; i++) {

                        $('#user').append("<tr onClick='mClickTR_user(this)'><td>" + data.d[i].USER_ID + "</td> <td>" + data.d[i].USER_NAME + "</td> <td>" + data.d[i].BUMON_CD + "</td></tr>");

                    }
                }
            },
            error: function (result) {
                //alert("Error login");

            }

        });

        di_user.dialog('open');

    });
    
    //部門チェンジイベント
    //部門名表示
    $('#d_bumon_id').on({
        change: function () {
            var msg_red = document.getElementById("msg_red");
            msg_red.innerHTML = '';
            var bumon_cd = document.getElementById("d_bumon_id").value;
            var wk_BUMON_NAME = document.getElementById("d_bumon_name");

            if (bumon_cd.length > 0) {
                //表示用関数
                 $('#d_bumon_name').func_bumon_nm(bumon_cd, wk_BUMON_NAME);
            } else {
                 $('#d_bumon_name').text('');
            }
        }
    });
    //部門選択画面オープン
    $('#bumon_serach').click(function (e) {

        var msg_red = document.getElementById("msg_red");
        msg_red.innerHTML = '';
        //データをクリア
        $('table#bumon *').remove();

        $.ajax({
            type: "POST",
            url: "Form_M_USER.aspx/GetJOSNData_Bumon",
            contentType: "application/json;charset=utf-8",
            data: {},
            dataType: "json",
            success: function (data) {

                if (data.d.length > 0) {
                    $('#bumon').append("<tr style='background-color:#0857E0'><th>BUMON_CD</th><th>BUMON_NAME</th><th>SAIMU_BMN</th></tr>");

                    for (var i = 0; i < data.d.length; i++) {
                        $('#bumon').append("<tr onClick='mClickTR_bumon(this)'><td>" + data.d[i].BUMON_CD + "</td> <td>" + data.d[i].BUMON_NM + "</td> <td>" + data.d[i].SAIMU_BMN + "</td></tr>");

                    }
                }
            },
            error: function (result) {
                //alert("Error login");

            }

        });

        di_bumon.dialog('open');

    });

    //税区分ラジオボタン変更時イベントリスナー
    $('input[name="z_kubun"]:radio').change(function () {
            if ($('input[name="z_kubun"]:eq(0)').is(':checked')) {
                $('#d_keihi').css('background-color', 'orange');
                $('#d_syozei').css('background-color', 'orange');
                calc_kingaku($('#d_kingaku'));
            } else {
                $('#d_keihi').css('background-color', 'lightgray');
                $('#d_syozei').css('background-color', 'lightgray');
                calc_kingaku($('#d_kingaku'));
            }
    });

    //ラジオボタン選択
　　//課税　
    $('#l_z_kubun0').click(function (e) {

        $('input[name=z_kubun]:eq(0)').prop('checked', true);
        $('#d_keihi').css('background-color', 'orange');
        $('#d_syozei').css('background-color', 'orange');

        calc_kingaku($('#d_kingaku'));

    });
    //非課税　
    $('#l_z_kubun1').click(function (e) {

        $('input[name=z_kubun]:eq(1)').prop('checked', true);
        $('#d_keihi').css('background-color', 'lightgray');
        $('#d_syozei').css('background-color', 'lightgray');

        calc_kingaku($('#d_kingaku'));

    });

    //その他
    $('#l_z_kubun2').click(function (e) {

        $('input[name=z_kubun]:eq(2)').prop('checked', true);
        $('#d_keihi').css('background-color', 'lightgray');
        $('#d_syozei').css('background-color', 'lightgray');

        calc_kingaku($('#d_kingaku'));

    });

    //科目チェンジイベント
    //科目名表示
    $('#d_kan_code').on({
        change: function () {
            var msg_red = document.getElementById("msg_red");
            msg_red.innerHTML = '';
            var kamoku_cd = document.getElementById("d_kan_code").value;
            var wk_KAMOKU_NAME = document.getElementById("d_kan_name");

            if (kamoku_cd.length > 0) {
                //科目名表示用関数
                //科目コードにより税区分の初期値をセット
                $('#d_kan_name').func_kamoku_nm_zei(kamoku_cd, wk_KAMOKU_NAME);
            } else {
                $('#d_kan_name').text('');
            }
            //金額編集、消費税計算
            calc_kingaku($('#d_kingaku'));

        }
    });
    //科目選択画面オープン
    $('#kan_serach').click(function (e) {
      
        var msg_red = document.getElementById("msg_red");
        msg_red.innerHTML = '';
        $('table#kamoku *').remove();
        $.ajax({
            type: "POST",
            url: "Form_SYKINDN_New.aspx/GetJOSNData_Kamoku",
            contentType: "application/json;charset=utf-8",
            data: {},
            dataType: "json",
            success: function (data) {

                if (data.d.length > 0) {
                    $('#kamoku').append("<tr style='background-color:#0857E0'><th>科目コード</th><th>科目名</th><th>税区分</th></tr>");

                    for (var i = 0; i < data.d.length; i++) {
                        $('#kamoku').append("<tr onClick='mClickTR_kamoku(this)'><td>" + data.d[i].KAMOKU_CD + "</td> <td>" + data.d[i].KAMOKU_NM + "</td><td>" + data.d[i].TAX_CD + "</td></tr>");

                    }
                    di_kamoku.dialog('open');

                } else {
                    $('#msg').text('データは０件でした');
                    di_msg.dialog('open');
                }
       
            },
            error: function (result) {
                //alert("Error login");

            }

        });
     
    });

    //内訳チェンジイベント
    //内訳名表示
    $('#d_uti_code').on({
        change: function () {
            var msg_red = document.getElementById("msg_red");
            msg_red.innerHTML = '';
            var wk_UTIWAKE_NAME = document.getElementById("d_uti_name");
            var utiwake_cd = document.getElementById("d_uti_code").value;
            var kamoku_cd = document.getElementById("d_kan_code").value;

            if (kamoku_cd.length === 0) {
                $(msg).text('');
                $(msg).text('科目コード指定してください');
                di_msg.dialog('open');
            } else {
                if (utiwake_cd.length > 0) {
                    //ユーザー表示用関数
                    $('#d_uti_name').func_utiwake_nm(kamoku_cd,utiwake_cd,wk_UTIWAKE_NAME);
                } else {
                    $('#d_uti_name').text('');
                }
            }
        }
    });
    //内訳選択画面オープン
    $('#uti_serach').click(function (e) {
        var msg_red = document.getElementById("msg_red");
        msg_red.innerHTML = '';
        var param_kamoku_cd = document.getElementById("d_kan_code").value;
        //データをクリア
        $('table#utiwake *').remove();

        if (param_kamoku_cd.length === 0) {
            $(msg).text('');
            $(msg).text('科目コード指定してください');
            di_msg.dialog('open');
        } else {

            var JSONdata = {
                code:param_kamoku_cd
            };

            $.ajax({
                type: "POST",
                url: "Form_SYKINDN_New.aspx/GetJOSNData_Utiwake",
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify(JSONdata),
                dataType: "json",
                success: function (data) {
                   
                    if (data.d.length > 0) {
                        $('#utiwake').append("<tr style='background-color:#0857E0'><th>内訳コード</th><th>内訳名</th></tr>");

                        for (var i = 0; i < data.d.length; i++) {
                            $('#utiwake').append("<tr onClick='mClickTR_utiwake(this)'><td>" + data.d[i].UCHI_CD + "</td> <td>" + data.d[i].UCHI_NM + "</td></tr>");

                        }
                        di_utiwake.dialog('open');

                    } else {

                        $('#msg').text('データは０件でした');
                        di_msg.dialog('open');

                    }

                },
                error: function (result) {
                    //alert("Error login");

                }

            });
        }
    });



    $('#d_kingaku').keypress(function () {

        //var newVal, val;
        //val = $(this).val().replace(/,/g, "");                  // (1)
        //newVal = val.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"); // (2)
        //$(this).val(newVal);

        //if ($('#d_tax').val() === '1' || $('#d_tax').val() === '2') {



        //} else {
        //    var keihi = $('#d_kingaku').val();
        //    //$('#d_keihi').val(newVal);
        //    //$('#d_gokei').val(newVal);
        //}

    });

    
    //金額の編集と消費税計算
    $('#d_kingaku').on({

        change: function () {
             
            //金額編集、消費税計算
            calc_kingaku($(this));

        }
    });

    //明細修正
    //行ダブルクリックで編集画面
    $("#MainContent_GridView1 td").dblclick(function () {

        //選択行をキープ
        var wk_GYO = document.getElementById("d_gyo");
        var row = $(this).closest('tr').index();
        wk_GYO.value = row;
    

        var col = this.cellIndex;
        var bumon_cd = $(this).closest("tr").find("td").eq(1).text();
        var shiharai= $(this).closest("tr").find("td").eq(2).text();
        var kamoku_cd = $(this).closest("tr").find("td").eq(3).text();
        var kamoku_nm = $(this).closest("tr").find("td").eq(4).text();
        var utiwake_cd = $(this).closest("tr").find("td").eq(5).text();
        var utiwake_nm = $(this).closest("tr").find("td").eq(6).text();
        var tekiyo = $(this).closest("tr").find("td").eq(7).text();
        var keihi = $(this).closest("tr").find("td").eq(8).text();
        var zei = $(this).closest("tr").find("td").eq(9).text();
        var kin = $(this).closest("tr").find("td").eq(10).text();


        //編集画面オープン
        di.dialog('open');

        var wk_BUMON_CD = document.getElementById("d_bumon_id");
        var wk_BUMON_NAME = document.getElementById("d_bumon_name");
        var wk_SHIHARAI = document.getElementById("d_shiharai");

        var wk_KAMOKU_CD = document.getElementById("d_kan_code");
        var wk_KAMOKU_NAME = document.getElementById("d_kan_name");

        var wk_UCHIWAKE_CD = document.getElementById("d_uti_code");
        var wk_UCHIWAKE_NAME = document.getElementById("d_uti_name");

        var wk_TEKIYO = document.getElementById("d_tekiyo");

        var wk_KINGAKU = document.getElementById("d_kingaku");
        var wk_KEIHI = document.getElementById("d_keihi");
        var wk_ZEI = document.getElementById("d_syozei");
        var wk_GOUKEI = document.getElementById("d_gokei");


        //部門名表示用関数
        //$('GridView1').func_bumon_nm(bumon_cd, wk_BUMON_NAME);
        $('d_bumon_name').func_bumon_nm(bumon_cd, wk_BUMON_NAME);

        wk_BUMON_CD.value = bumon_cd;
        wk_SHIHARAI.value = shiharai;
        wk_KAMOKU_CD.value = kamoku_cd;
        wk_KAMOKU_NAME.innerHTML = kamoku_nm;
        wk_UCHIWAKE_CD.value = utiwake_cd;
        wk_UCHIWAKE_NAME.innerHTML=  utiwake_nm;
        wk_TEKIYO.value = tekiyo;

        wk_KINGAKU.value = kin;

        wk_KEIHI.value = keihi;
        wk_ZEI.value = zei;
        wk_GOUKEI.value = kin;

    
    });




});
    //ユーザー選択するイベント
function mClickTR_user(obj) {

        var cd = document.getElementById('d_user_cd');
        var nm = document.getElementById('d_user_name');
        console.log(cd);
        console.log(nm);
        cd.value = obj.cells[0].innerHTML;
        nm.value = obj.cells[1].innerHTML;
        $(nm).css('color', 'black');

}
//部門選択するイベント
function mClickTR_bumon(obj) {
    var cd = document.getElementById('d_bumon_id');
    var nm = document.getElementById('d_bumon_name');
    console.log(cd);
    console.log(nm);
    cd.value = obj.cells[0].innerHTML;
    nm.innerHTML = obj.cells[1].innerHTML;
    $(nm).css('color', 'black');

}

//科目選択イベント
function mClickTR_kamoku(obj) {
    var cd = document.getElementById('d_kan_code');
    var nm = document.getElementById('d_kan_name');
    var tax = document.getElementById('d_tax');
    console.log(cd);
    console.log(nm);
    cd.value = obj.cells[0].innerHTML;
    nm.innerHTML = obj.cells[1].innerHTML;
    tax.value = obj.cells[2].innerHTML;

    $(nm).css('color', 'black');
    $('#d_uti_code').val('');
    $('#d_uti_name').text('');
    //税区分をセット
    $('#d_keihi').css('background-color', 'lightgray');
    $('#d_syozei').css('background-color', 'lightgray');

    if ($('#d_tax').val() === '1' || $('#d_tax').val() === '2') {
        $('input[name=z_kubun]:eq(0)').prop('checked', true);
        $('#d_keihi').css('background-color', 'orange');
        $('#d_syozei').css('background-color', 'orange');

    } else if ($('#d_tax').val() === '3') {
        $('input[name=z_kubun]:eq(1)').prop('checked', true);
    } else {
        $('input[name=z_kubun]:eq(2)').prop('checked', true);
    }

}


//内訳選択するイベント
function mClickTR_utiwake(obj) {
    var cd = document.getElementById('d_uti_code');
    var nm = document.getElementById('d_uti_name');
    console.log(cd);
    console.log(nm);
    cd.value = obj.cells[0].innerHTML;
    nm.innerHTML = obj.cells[1].innerHTML;
    $(nm).css('color', 'black');
 
}

</script>
<div id="Container">
      <div class="index1">
      出金伝票入力（新規） </div>
<div>
<asp:TextBox type="hidden" class="syubeu" ID="syubetu" runat="server" BorderStyle="None" ></asp:TextBox>
</div>
<div class="header0">
<asp:label runat="server" class="denban_lbl">伝票番号</asp:label><asp:TextBox class="denban" ID="denban1" runat="server" ></asp:TextBox>
<asp:label runat="server" class="zandaka_lbl">残高</asp:label><asp:TextBox class="zandaka" ID="zandaka" runat="server" ></asp:TextBox>
    <br />
   <asp:Panel class="panel" runat="server" BorderStyle="Solid"  BackColor ="WhiteSmoke">
   <table class="header_group1">
    <tr>
     <td>購入日　<asp:TextBox class="div_kounyu_date" id="kounyu_date" runat="server"></asp:TextBox></td>
     <td></td> 
    </tr>
      <tr>
         <td>社員氏名<input type="text" name="d_user_cd" id="d_user_cd"/><input id="user_serach" type="button" value="..."/ style="width:28px"></td>
         <td><input type="text" name="d_user_name" id="d_user_name" style="width:150px"/></td>
       </tr>
</table>
        </asp:Panel>
    </div>
 <div class="meisai" >
 <div id="hidden">
 </div>
<asp:ListView ID="ListView1" runat="server">
  <LayoutTemplate>
      <table id="highlight_after" class="fixdetail">
      <thead>
    <tr>
      <th>行</th><th>部門ｺｰﾄﾞ</th><th>支払先</th><th>科目ｺｰﾄﾞ</th><th>科目名</th><th>内訳ｺｰﾄﾞ</th><th>内訳名</th><th>摘要</th><th>経費</th><th>消費税</th><th>合計金額</th>
    </tr>
</thead>
  <tbody>
<asp:placeholder runat="server" ID="itemPlaceholder">

</asp:placeholder>
  </tbody>
  </table>
  </LayoutTemplate>
  <ItemTemplate>
      <tr id="<%#CType(Container, ListViewItem).DataItemIndex + 1%>" class="color<%#(CType(Container, ListViewItem).DataItemIndex + 1) Mod 2%>">
          <td><%# Eval("行") %></td>
          <td><input type="text" id="bumon_cd_s" value='<%# Eval("部門ｺｰﾄﾞ") %>'/></td>
          <td><%# Eval("支払先") %></td>
          <td><%# Eval("科目ｺｰﾄﾞ") %></td>
          <td><%# Eval("科目名") %></td>
          <td><%# Eval("内訳ｺｰﾄﾞ") %></td>
          <td><%# Eval("内訳名") %></td>
          <td><%# Eval("摘要") %></td>
          <td><%# Eval("経費") %></td>
          <td><%# Eval("消費税") %></td>
          <td><%# Eval("合計金額") %></td>
      </tr>
  </ItemTemplate>

</asp:ListView>

<p></p>
<asp:button id="gyo_new" runat="server" text="行追加"/>
<asp:button runat="server" text="行削除"/>
<p></p>
<div>
<input type="button" id="button_kakunin" value="確認" style="height:50px"/>
<asp:button runat="server"  id="button_kousin" text="更新" style="height:50px"/>
<asp:button class="bottun_close"  runat="server" text="閉じる" style="height:50px"/>
</div>
 </div>      
<div id="dialog_user" title="ユーザーマスタ">
<table id="user" border="1">
</table>
</div>
<div id="dialog_msg" title="メッセージ">
<label id="msg" style="font-size:small;"></label> 
<input id="msg_status" type="hidden" />
</div>
<!--新規登録画面の開始-->
<div id="dialog_syukin" class="dialog_syukin" title="出金伝票新規">
 <input type="hidden" id="d_gyo" />
 <table border ="0">
 <tr>
 <td ><label>部門</label></td><td><input type="text"id="d_bumon_id"/><input id="bumon_serach" type="button" value="..."/ style="width:28px"></td>
 <td class="nowrap"><label id="d_bumon_name" ></label></td>
 </tr>
<tr><td>　</td><td></td></tr>
<tr>
<td><label>支払</label></td><td><input type="text" id="d_shiharai" style="width:300px" />
</tr>
<tr><td>　</td></tr>
<tr>
<td><label>勘定科目</label></td><td><input type="text" id="d_kan_code" /><input id="kan_serach" type="button" value="..."/ style="width:28px"></td>
<td class="nowrap"><label id="d_kan_name"></label></td>
</tr>
<tr><td>　</td></tr>
<tr>
<td><label>内訳</label></td><td><input type="text" id="d_uti_code" /><input id="uti_serach" type="button" value="..."/ style="width:28px"></td>
<td class="nowrap"><label id="d_uti_name"></label></td>
</tr>
<tr><td>　</td></tr>
<tr>
<td><label>摘要</label></td><td><input type="text" id="d_tekiyo" style="width:300px" />
</tr>
</table>
<table>
<tr><td>　</td></tr>
<tr>
     <td><input type="radio" id="d_z_kubun0" name="z_kubun" value="0" style="width:10px" /><label id="l_z_kubun0" style="width:100px;color:darkgreen ">課税</label></td>
     <td><input type="radio" id="d_z_kubun1" name="z_kubun" value="1" style="width:10px" /><label id="l_z_kubun1" style="width:100px;color:darkgreen">非課税</label></td>
     <td><input type="radio" id="d_z_kubun2" name="z_kubun" value="2" style="width:10px" /><label id="l_z_kubun2" style="width:100px;color:darkgreen">その他</label></td>
</tr>
</table>
<table>
<tr><td>　</td></tr>
<tr>
<td><label>金額　　</label></td><td><input type="text" id="d_kingaku" style="text-align:right;width:150px" />
</tr>
</table>
<input type="hidden" id="d_tax"/>
<input type="text" id="d_zeiritu"/>
    <p>　</p>
    <asp:panel runat="server" BorderStyle="Solid" Width="300px" BorderWidth="1" >
        <table style="margin:5px">
            <tr>
                <td><label style="color:darkgreen">経費　　</label></td><td><input type="text" id="d_keihi" style="text-align:right;width:150px"/></td>
            </tr>
            <tr>
                <td><label style="color:darkgreen">消費税　</label></td><td><input type="text" id="d_syozei" style="text-align:right;width:150px"/></td>
            </tr>
            <tr>
                <td><label style="color:darkgreen">合計額　</label> </td><td><input type="text" id="d_gokei" style="text-align:right;width:150px"/></td>
            </tr>
        </table>
        </asp:panel>
<p></p>
<div id="msg_red" style="display:none; font-size:smaller ;color:red;"></div>
</div> 
<!--新規登録画面の終了-->
<div id="dialog_bumon" title="部門マスタ">
<table id="bumon" border="1">
</table>
</div>
<div id="dialog_kamoku" title="科目マスタ">
<table id="kamoku" border="1">
</table>
</div>
<div id="dialog_utiwake" title="内訳マスタ">
<table id="utiwake" border="1">
</table>
</div>
</div>
</asp:Content>
