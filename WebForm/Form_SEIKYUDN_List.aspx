<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Form_SEIKYUDN_List.aspx.vb" Inherits="KeihiWeb.Form_SEIKYUDN_List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<style type="text/css">
   table#trade tr:hover {
    background-color: #CC99FF; /* 行の背景色 */
    }
   table#kamoku tr:hover {
    background-color: #CC99FF; /* 行の背景色 */
    }
    table#utiwake tr:hover {
    background-color: #CC99FF; /* 行の背景色 */

    }
    .grd_01{
      column-width:60px;
        line-height:25px;
             }

    .grd1_02{
      column-width:40px;
        line-height:25px;
    }
    .grd1_03{
        column-width:100px;
        line-height:25px;
    }
    .grd1_keihi{
        column-width:120px;
        line-height:25px;
    }
    .grd1_zei{
        column-width:120px;
        line-height:25px;
    }
    .grd1_kin{
        column-width:120px;
        line-height:25px;
    }
   .grd1_tradecd{
        display:none;
    }
   
    .grd1_manageno{
        display:none;
    }
   .grd1_jibumoncd{
        display:none;
    }
    .grd1_seqcd{
        display:none;
    }
    
    .grd1_notes{
        column-width:180px;
        line-height:25px;
        white-space: nowrap;
        text-overflow:ellipsis;
        overflow:hidden;
     }
  
   .grd2_manageno{
        display:none;
    }
   .grd2_jibumoncd{
        display:none;
    }
   .grd2_tradecd{
        display:none;
    }
   .grd2_seqcd{
        display:none;
    }
    
    td.nowrap {
      white-space: nowrap;
    }
    td  {
          word-wrap: normal;
          white-space: nowrap;
    }
   .header0{
          position:relative;
          }

  
  .div_SearchButton {
       position:absolute;
       left:420px;
   }
  .div_ClearButton {
       position:absolute;
   left:500px;
       }
  
   .btnClose {
       position:absolute;
       right:45px;
   }
   .panel{
       position:absolute;
       height:120px;
       top:10px;
       padding-top:10px;
   }
   .meisai{
          position:relative;
          top:110px;
   }
   .goukei_ran {
       position:absolute;
       left:0px;
   }
   
   .meisai_meisai{
        position:relative;
        top:0px;
        display:none;
     }
    #d_trade_cd{
          position:relative;
          left:9px;
          width:100px; 
          margin-left:5px;
   }
   #trade_serach{
          position:relative;
          left:15px;
     
   }
   .input_date1{
          position:relative ;
          left:9px;
          width:100px; 
          margin-left:5px;
   }
   .kara{
          position:relative ;
          left:10px;
          margin-left:5px;
   }
  .input_date2{
          position:relative ; 
          left:10px;
          width:100px; 
          margin-left:5px;
   }
   .trade {
     white-space: nowrap;
 }

</style>
<script type="text/javascript" src="../Scripts/WebForms/STSCommon/common.js" ></script>
<script type="text/javascript" src="../Scripts/WebForms/STSCommon/keihi_trade.js" ></script>
<script type="text/javascript" src="../Scripts/WebForms/STSCommon/keihi_bumon.js" ></script>
<script type="text/javascript" src="../Scripts/WebForms/STSCommon/keihi_kamoku.js" ></script>
<script type="text/javascript" src="../Scripts/WebForms/STSCommon/keihi_zeiritu.js" ></script>
<style type="text/css">
     /* --- マウスオーバー時 --- */
table#bumon tr:hover {
background-color: #CC99FF; /* 行の背景色 */
}
</style>

<script type="text/javascript">

$(document).ready(function () {

    //合計欄の計算

    　　syukei_calc(16, 17, 18);

 
});
$(function () {
    //エラーチェック用グローバル変数
    var errchk_date1;
    var errchk_date2;

    ///**********　　　
    //初期セット
    ///**********　　　
    //
    //////エンターによる命令防止

    $('#Container').keypress(function (ev) {
        if ((ev.which && ev.which === 13) || (ev.keyCode && ev.keyCode === 13)) {
            return false

        } else {
            return true

        }
    });

        $("input[type = text][id='<%=input_date1.ClientID%>']").bind("keydown", function (e) {
        var n = $("input[type = text][id='<%=input_date1.ClientID%>']").length;
        if (e.which == 13 || e.which == 9) {
            e.preventDefault();
            $('#<%=input_date2.ClientID%>').focus();
         
        }
    });
     $("input[type = text][id='<%=input_date2.ClientID%>']").bind("keydown", function (e) {
        var n = $("input[type = text][id='<%=input_date2.ClientID%>']").length;
        if (e.which == 13 || e.which == 9) {
            e.preventDefault();
            $('#d_trade_cd').focus();

        }
    });

    $('#d_trade_cd').keypress(function (e) {
        if (e.which == 13 || e.which == 9) {
            // ここに処理を記述
            $('#trade_serach').focus();
            return false;
        }
    });




    $('#title').css('background-color', '#D3EDFB');
    $('#title').css('border-left-color', 'blue');

    //グリッドの調整
    var tr = $("#MainContent_GridView1 tr");
    for (var i = 0, l = tr.length ; i < l; i++) {

        var cells = tr.eq(i).children();//1行目から順にth、td問わず列を取得
        cells.eq(2).addClass('grd1_tradecd');
        //cells.eq(11).addClass('grd1_notes');
        //cells.eq(14).addClass('grd1_keihi');
        //cells.eq(15).addClass('grd1_zei');
        //cells.eq(16).addClass('grd1_kin');
        cells.eq(19).addClass('grd1_jibumoncd');
        cells.eq(20).addClass('grd1_manageno');
    }
    
    var tr = $("#MainContent_GridView2 tr");
    for (var i = 0, l = tr.length ; i < l; i++) {

        var cells = tr.eq(i).children();//1行目から順にth、td問わず列を取得
        cells.eq(2).addClass('grd2_tradecd');
        cells.eq(9).addClass('grd2_jibumoncd');
        cells.eq(10).addClass('grd2_manageno');
        cells.eq(11).addClass('grd2_seqcd');
    }


    var status_disp = $('#<%=disp.ClientID%>').val();
    if (status_disp.length === 0) {
        $('#<%=disp.ClientID%>').val('2');
    }
    //明細表示ヘッダ表示切替
    if (status_disp === '1') {
        $('#<%=GridView1.ClientID%>').show();
        $('#<%=GridView2.ClientID%>').hide();
        $('#<%=Change_disp.ClientID%>').val('<<');
    }
　　//明細表示ヘッダ表示切替
    if (status_disp === '2') {
        $('#<%=GridView1.ClientID%>').hide();
        $('#<%=GridView2.ClientID%>').show();
        $('#<%=Change_disp.ClientID%>').val('>>');
    }

    $('#title').css('background-color', '#5BC85B');
    $('#title').css('border-left-color', 'Green');

  
    $.datepicker.setDefaults($.datepicker.regional['ja']);
    $('#<%=input_date1.ClientID%>').datepicker({
        dateFormat: 'yy/mm/dd'
    });

    $('#<%=input_date2.ClientID%>').datepicker({
      dateFormat: 'yy/mm/dd',
    });

    var datefrom = $('#<%=date_from.ClientID%>').val();
    var dateto = $('#<%=date_to.ClientID%>').val();
   
    $('#<%=input_date1.ClientID%>').datepicker('setDate', new Date(Date.parse(datefrom)));
    $('#<%=input_date2.ClientID%>').datepicker('setDate', new Date(Date.parse(dateto)));


    $('#d_torihki_name').css('ime-mode', 'active');
    $('#d_kana_name').css('ime-mode', 'active');
    $('#d_trade_cd').css('ime-mode', 'inactive');
    $('#<%=input_date1.ClientID%>').css('ime-mode', 'inactive');
    $('#<%=input_date2.ClientID%>').css('ime-mode', 'inactive');


     //購入日を変更したら更新ボタンをＦＡＬＳＥ
    $('#<%=input_date1.ClientID%>').on({
        change: function () {
          
           $('#<%=SearchButton.ClientID%>').prop('disabled', false);
           errchk_date1 = '';

           if ($('#<%=input_date1.ClientID%>').val().length > 10) {
               $('#<%=SearchButton.ClientID%>').prop('disabled', true);

                $('#msg').text('開始入力日の値が不正です(yyyy/MM/dd)');
                $('#err_msg_lbl').val('開始入力日エラー');
                errchk_date1 ='1';
                di_msg.dialog('open');
                return false;
           } else {
               $('#err_msg_lbl').val('');
           }

            var datestr = $('#<%=input_date1.ClientID%>').val();

            if (datestr.match(/[0-9]{8}/)) {
                str1 = datestr.substring(0, 4) + "/" + datestr.substring(4, 6) + "/" + datestr.substring(6, 8)
                $('#<%=input_date1.ClientID%>').val(str1);
                datestr = $('#<%=input_date1.ClientID%>').val();
            } else {
            }
            

            var vYear = datestr.substr(0, 4) - 0;
            // Javascriptは、0-11で表現
            var vMonth = datestr.substr(5, 2) - 1;
            var vDay = datestr.substr(8, 2) - 0;
            // 月,日の妥当性チェック
            if (vMonth >= 0 && vMonth <= 11 && vDay >= 1 && vDay <= 31) {
                var vDt = new Date(vYear, vMonth, vDay);
                if (isNaN(vDt)) {
                     $('#<%=SearchButton.ClientID%>').prop('disabled', true);
                    $('#msg').text('開始入力日の値が不正です(yyyy/MM/dd)');
                    $('#err_msg_lbl').val('開始入力日エラー');
                    errchk_date1 = '1';
                    di_msg.dialog('open');
                    return false;
                } else if (vDt.getFullYear() == vYear
                 && vDt.getMonth() == vMonth
                 && vDt.getDate() == vDay) {
                    $('#err_msg_lbl').val('');
                } else {
                    $('#<%=SearchButton.ClientID%>').prop('disabled', true);
                    $('#msg').text('開始入力日の値が不正です(yyyy/MM/dd)');
                    errchk_date1 = '1';
                    $('#err_msg_lbl').val('開始入力日エラー');
                    di_msg.dialog('open');
                    return false;
                }
            } else {
                $('#<%=SearchButton.ClientID%>').prop('disabled', true);
                $('#err_msg_lbl').val('開始入力日エラー');
                errchk_date1 = '1';
                $('#msg').text('開始入力日の値が不正です(yyyy/MM/dd)');
                di_msg.dialog('open');

                return false;
            }

             if ($('#<%=input_date1.ClientID%>').val().length < 10) {
                 $('#<%=SearchButton.ClientID%>').prop('disabled', true);
                 errchk_date1 = '1';
                 $('#msg').text('開始入力日の値が不正です(yyyy/MM/dd)');
                 $('#err_msg_lbl').val('開始入力日エラー');
                di_msg.dialog('open');
                return false;
             } else {
                 $('#err_msg_lbl').val('');

             }
             var datefrom_chg = $('#<%=input_date1.ClientID%>').val();
             $('#<%=input_date2.ClientID%>').focus();                 
        }
    });

    $('#<%=input_date2.ClientID%>').on({
        change: function () {
           $('#<%=SearchButton.ClientID%>').prop('disabled', false);
           errchk_date2 = '';

           if ($('#<%=input_date2.ClientID%>').val().length > 10) {
                $('#<%=SearchButton.ClientID%>').prop('disabled', true);
               errchk_date2 = '1';
               $('#msg').text('終了入力日の値が不正です(yyyy/MM/dd)');
                $('#err_msg_lbl').val('終了入力日エラー');
                di_msg.dialog('open');
                return false;
            }

            var datestr = $('#<%=input_date2.ClientID%>').val();

            if (datestr.match(/[0-9]{8}/)) {
                str1 = datestr.substring(0, 4) + "/" + datestr.substring(4, 6) + "/" + datestr.substring(6, 8)
                $('#<%=input_date2.ClientID%>').val(str1);
                datestr = $('#<%=input_date2.ClientID%>').val();
            } else {
            }
            

            var vYear = datestr.substr(0, 4) - 0;
            // Javascriptは、0-11で表現
            var vMonth = datestr.substr(5, 2) - 1;
            var vDay = datestr.substr(8, 2) - 0;
            // 月,日の妥当性チェック
            if (vMonth >= 0 && vMonth <= 11 && vDay >= 1 && vDay <= 31) {
                var vDt = new Date(vYear, vMonth, vDay);
                if (isNaN(vDt)) {
                    $('#<%=SearchButton.ClientID%>').prop('disabled', true);
                    $('#msg').text('終了入力日の値が不正です(yyyy/MM/dd)');
                    errchk_date2 = '1';
                    $('#err_msg_lbl').val('終了入力日エラー');
                    di_msg.dialog('open');
                    return false;
                } else if (vDt.getFullYear() == vYear
                 && vDt.getMonth() == vMonth
                 && vDt.getDate() == vDay) {
                } else {
                    $('#<%=SearchButton.ClientID%>').prop('disabled', true);
                    errchk_date2 = '1';
                    $('#msg').text('終了入力日の値が不正です(yyyy/MM/dd)');
                    $('#err_msg_lbl').val('終了入力日エラー');
                    di_msg.dialog('open');
                    return false;
                }
            } else {
                $('#<%=SearchButton.ClientID%>').prop('disabled', true);
                errchk_date2 = '1';
                $('#err_msg_lbl').val('終了入力日エラー');
                $('#msg').text('終了入力日の値が不正です(yyyy/MM/dd)');
                di_msg.dialog('open');

                return false;
            }

             if ($('#<%=input_date2.ClientID%>').val().length < 10) {
                 errchk_date2 = '1';
                 $('#<%=SearchButton.ClientID%>').prop('disabled', true);
                $('#msg').text('終了入力日の値が不正です(yyyy/MM/dd)');
                $('#err_msg_lbl').val('終了入力日エラー');
                di_msg.dialog('open');
                return false;
            }
            $('#d_trade_cd').focus();
         
        }
    });


    var wk_GYO = document.getElementById("d_gyo");
    wk_GYO.value = '';


    $('#d_trade_cd').val($('#<%=h_tradecd.ClientID%>').val());
    $('#d_trade_name').val($('#<%=h_tradename.ClientID%>').val());
    $('#d_trade_address').val($('#<%=h_tradeaddress.ClientID%>').val());

  
    $('#<%=input_date1.ClientID%>').focus(function () {
        $(this).select();
    });
    $('#<%=input_date2.ClientID%>').focus(function () {
        $(this).select();
    });

//検索ボタン
    $('#<%=SearchButton.ClientID%>').click(function () {
        if (errchk_date1 === '1' || errchk_date2 === '1') {

            $('#msg').text('エラーが存在します');
            di_msg.dialog('open');

            return false;

        }

    });
                   

    $('#d_trade_cd').focus(function () {
        $(this).select();
    });

  
    //取引先選択画面
    var di_trade =$('#dialog_trade');
    di_trade.dialog({
        modal: true,
        width: 700,
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
        width: 300,
        height: 200,
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

    $('#trade').click(function () {
        di_trade.dialog('close');

    });

        //取引き先チェンジイベント
    //取引先名表示
    $('#d_trade_cd').on({
        change: function () {
       
            var trade_cd = document.getElementById("d_trade_cd").value;
         
            var wk_TRADE_NAME = document.getElementById("d_trade_name");
            var wk_TRADE_ADDRESS = document.getElementById("d_trade_address");
            var ad = document.getElementById("d_trade_address");
            ad.value = '';

            if (trade_cd.length > 0) {
                //ユーザー表示用関数
                //$('#d_user_name').func_user_nm(user_cd, wk_USER_NAME);
                $('#d_trade_cd').func_trade_nm(trade_cd, wk_TRADE_NAME, ad);

            } else {
                $('#d_trade_name').val('');
                $('#d_trade_address').val('');
            }
        }
    });

    //クリアボタン
    $('#<%=SearchClear.ClientID%>').click(function (e) {

        var url_wk = 'Form_SEIKYUDN_New.aspx/GetCloseKikan';
        var param_jibumoncd = document.getElementById("auth_bumon_cd").value;
        var JSONdata = {
            code: param_jibumoncd
        };
     
        $.ajax({
            type: "POST",
            url: url_wk,
            datatype: 'json',
            data: JSON.stringify(JSONdata),
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (result) {

                var member = result.d;
                //結果をセット
                //wk_BUMON_NAME.innerHTML = member[0].BUMON_NM;

                $('#<%=input_date1.ClientID%>').val(member[0].CLOSEDATE_S)
                $('#<%=input_date2.ClientID%>').val(member[0].CLOSEDATE_E)
           
                return result;

            },
            error: function (data, errorThrown) {
                alert('異常終了！！' + errorThrown);
                return false;

            }
        }).done(function (msg) {

        });


        $('#d_trade_cd').val('');
        $('#d_trade_name').val('');
        $('#d_trade_address').val('');
        var datefrom = $('#<%=h_date_from.ClientID%>').val();
        var dateto = $('#<%=h_date_to.ClientID%>').val();
     
        return false;

    });



    //取引先選択画面オープン
    $('#trade_serach').click(function (e) {
        $('#err_msg_lbl').val('');
        errchk_date = '';

        //検索データをクリア
        $('#d_torihki_name').val('');
        $('#d_kana_name').val('');

        $('table#trade *').remove();
        $.ajax({
            type: "POST",
            url: "Form_SEIKYUDN_New.aspx/GetJOSNData_Trade",
            contentType: "application/json;charset=utf-8",
            data: {},
            dataType: "json",
            success: function (data) {
                if (data.d.length > 0) {

                    $('#trade').append("<tr style='background-color:#6699FF'><th >コード</th><th>取引先</th><th>カナ</th><th>住所</th><th>口座番号</th><th>口座名</th></tr>");
             
                    for (var i = 0; i < data.d.length; i++) {

                        $('#trade').append("<tr onClick='mClickTR_trade(this)'><td>" + data.d[i].TRADE_CD + "</td> <td>" + data.d[i].TRADE_NM + "</td><td>" + data.d[i].TRADE_KN + "</td> <td>" + data.d[i].ADDRESS1 + "</td><td>" + data.d[i].ACC_NUM + "</td><td>" + data.d[i].ACC_NAME + "</td></tr>");

                    }
                }
            },
            error: function (result, errorThrown) {
                $('#msg').text('異常終了！！' + errorThrown);
                di_msg.dialog('open');
                return false;

                //alert("Error login");

            }

        });

        di_trade.dialog('open');

    });
    
    //取引先選択画面オープン
    $('#search_button_torihki').click(function (e) {

        var name_wk = $('#d_torihki_name').val();
        var kana_wk = $('#d_kana_name').val();
        $('#err_msg_lbl').val('');

        //データをクリア
        $('table#trade *').remove();

        var JSONdata = {
            name: name_wk,
            kana: kana_wk
        };

        $.ajax({
            type: "POST",
            url: "Form_SEIKYUDN_New.aspx/GetJOSNData_Trade2",
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify(JSONdata),
            dataType: "json",
            success: function (data) {
                if (data.d.length > 0) {
                    $('#trade').append("<tr style='background-color:#6699FF'><th >コード</th><th>取引先</th><th>カナ</th><th>住所</th><th>口座番号</th><th>口座名</th></tr>");
             
                    for (var i = 0; i < data.d.length; i++) {

                        $('#trade').append("<tr onClick='mClickTR_trade(this)'><td>" + data.d[i].TRADE_CD + "</td> <td>" + data.d[i].TRADE_NM + "</td><td>" + data.d[i].TRADE_KN + "</td> <td>" + data.d[i].ADDRESS1 + "</td><td>" + data.d[i].ACC_NUM + "</td><td>" + data.d[i].ACC_NAME + "</td></tr>");


                    }
                }
            },
            error: function (result, errorThrown) {
                $('#msg').text('異常終了！！' + errorThrown);
                di_msg.dialog('open');
                return false;

                //alert("Error login");

            }

        });


        di_trade.dialog('open');

    });



    //明細表示ボタンクリック
    $('#<%=Button_Disp.ClientID%>').click(function (e) {
        var chk = '0';
        if ($('#<%=Change_disp.ClientID%>').val() === '<<') {
           $('#<%=GridView1.ClientID%> input:radio').each(function () {
               if ($(this).prop('checked') === true) {

                   chk = '1';
                   //データ
                   var wk_GYO = document.getElementById("d_gyo");
                   var row = parseInt(wk_GYO.value);

                }
           });
        } else {
              $('#<%=GridView2.ClientID%> input:radio').each(function () {
               if ($(this).prop('checked') === true) {

                  chk = '1';
                   //データ
                  var wk_GYO = document.getElementById("d_gyo");
                  var row = parseInt(wk_GYO.value);

                  var tr = $("#MainContent_GridView2 tr");
                  var cells = tr.eq(row).children();
                  var inputdate = cells.eq(4).text();
   
                   
                }
           });
        }

        if (chk === '1') {

            var wk_GYO = document.getElementById("d_gyo");

        } else {

            $('#msg').text('行選択されていません');
            di_msg.dialog('open');
            return false;

        }

    });

    //
    //行クリック選択ラジオンボタンの解除とチェック
    //
    $("#MainContent_GridView1 td").click(function () {

         $('#<%=GridView1.ClientID%> input:radio').each(function () {
                $(this).prop('checked', false);
            });
         $(this).closest("tr").find("td input:radio").each(function () {
               $(this).prop('checked', true);
         });

        //選択行をキープ
         var wk_GYO = document.getElementById("d_gyo");
         var row = $(this).closest('tr').index();
         wk_GYO.value = row;
    
    });
  

     $("#MainContent_GridView2 td").click(function () {

         $('#<%=GridView2.ClientID%> input:radio').each(function () {
                $(this).prop('checked', false);
            });
         $(this).closest("tr").find("td input:radio").each(function () {
               $(this).prop('checked', true);
         });

        //選択行をキープ
         var wk_GYO = document.getElementById("d_gyo");
         var row = $(this).closest('tr').index();
         wk_GYO.value = row;
    
    });
  


    //明細修正
    //行ダブルクリックで編集画面
    $("#MainContent_GridView1 td").dblclick(function () {
       

        $('#<%=GridView1.ClientID%> input:radio').each(function () {
                $(this).prop('checked', false);
            });
         $(this).closest("tr").find("td input:radio").each(function () {
               $(this).prop('checked', true);
         });

         $('#<%=Button_Disp.ClientID%>').trigger('click');

      
    });

     //行ダブルクリックで編集画面
    $("#MainContent_GridView2 td").dblclick(function () {
       
        $('#<%=GridView2.ClientID%> input:radio').each(function () {
                $(this).prop('checked', false);
            });
         $(this).closest("tr").find("td input:radio").each(function () {
               $(this).prop('checked', true);
         });
     
        //選択行をキープ
         var wk_GYO = document.getElementById("d_gyo");
         var row = $(this).closest('tr').index();
         wk_GYO.value = row;

      
         $('#<%=Button_Disp.ClientID%>').trigger('click');
    
    });


     $('#<%=Change_disp.ClientID%>').click(function () {
         if ($('#<%=Change_disp.ClientID%>').val() === '<<') {
             $('#<%=Change_disp.ClientID%>').val('>>');
             $('#<%=disp.ClientID%>').val('2');
         } else {
                $('#<%=Change_disp.ClientID%>').val('<<');
             $('#<%=disp.ClientID%>').val('1');
         }
        
         $('#<%=GridView1.ClientID%>').toggle();
         $('#<%=GridView2.ClientID%>').toggle();
         return false;
     });
   
    $('input[readonly="readonly"]').on('focus', function () {
        $(this).blur();
    });

  
    var trade_cd01 = document.getElementById("d_trade_cd").value;
    var wk_NAME = document.getElementById("d_trade_name");
    var wk_AD = document.getElementById("d_trade_address");
    if (trade_cd01.length > 0) {
        //取引先表示用関数
        $('#d_trade_name').func_trade_nm(trade_cd01, wk_NAME, wk_AD);
  
    } else {
        $('#d_trade_name').val('');
    }



});

    //ユーザー選択するイベント
function mClickTR_trade(obj) {

        var cd = document.getElementById("d_trade_cd");
        var nm = document.getElementById("d_trade_name");
        var ad = document.getElementById("d_trade_address");
        cd.value = obj.cells[0].innerHTML;
        nm.value = obj.cells[1].innerHTML;
        ad.value = obj.cells[3].innerHTML;

        $(nm).css('color', 'black');

}

</script>
<div id="Container">
 <div id="hidden5">
 </div>
    <div id="title" class="index1">
      請求伝票一覧 </div>
<div>

<asp:TextBox type="hidden" ID="h_date_from" runat="server"  ></asp:TextBox>
<asp:TextBox type="hidden" ID="h_date_to" runat="server"  ></asp:TextBox>
<asp:TextBox type="hidden" ID="h_tradecd" runat="server"  ></asp:TextBox>
<asp:TextBox type="hidden" ID="h_tradename" runat="server"  ></asp:TextBox>
<asp:TextBox type="hidden" ID="h_tradeaddress" runat="server"  ></asp:TextBox>

<asp:TextBox type="hidden" ID="auth_user_id" runat="server"  ></asp:TextBox>
<asp:TextBox type="hidden" ID="auth_user_name" runat="server"  ></asp:TextBox>
<asp:TextBox type="hidden" class="syubeu" ID="syubetu" runat="server" BorderStyle="None" ></asp:TextBox>
<asp:TextBox type="hidden" ID="date_from" runat="server"  ></asp:TextBox>
<asp:TextBox type="hidden" ID="date_to" runat="server"  ></asp:TextBox>
<asp:TextBox type="hidden" ID="disp" runat="server"  ></asp:TextBox>
<input type="hidden" id="d_gyo" name="d_gyo" />
 </div>
<div class="header0">
<br />
   <asp:Panel class="panel" runat="server" BorderStyle="Solid"  width="100%" BackColor ="WhiteSmoke">
   <table class="header_group1">
    <tr>
     <td>入力日<asp:TextBox class="input_date1 div_input_date datepicker input-sm" id="input_date1" runat="server"></asp:TextBox><label class="kara">～</label><asp:TextBox class="input_date2 div_input_date datepicker input-sm" id="input_date2" runat="server"></asp:TextBox></td>
    </tr>
      <tr>
         <td>取引先<input type="text" name="d_trade_cd" id="d_trade_cd" class="input-sm" /><input id="trade_serach" type="button" class="btn btn-default" value="..." >
         <input type="text" name="d_trade_name" id="d_trade_name" tabindex="-1" class="input-sm" style="font-size:14px ; margin-left:10px;background-color:whitesmoke;border-style: none;width:200px" readonly="readonly" />
         <input type="text" name="d_trade_address" id="d_trade_address" tabindex="-1" class="input-sm" style="font-size:14px;background-color:whitesmoke;border-style: none;width:300px" readonly="readonly" /></td>
      </tr>
     <tr>
         <td><asp:button id="SearchButton" cssClass="div_SearchButton btn btn-default" runat="server" text="検索" />
             <asp:button id="SearchClear" cssClass="div_ClearButton btn btn-default" runat="server" text="クリア" />
　　　　 </td>
       </tr>
</table>
</asp:Panel>
</div>
<div class="meisai" >
<asp:button id="Change_disp" runat="server" cssclass="btn btn-default" text=">>" style="font-size:small"/>
<asp:Panel ID="Panel1" runat="server" ScrollBars="Both"  HorizontalAlign="Center" Width="100%"
        Height="280px" >
 <asp:GridView cssclass="meisai_meisai"　runat="server" ID="GridView1" Width="100%" >
           <Columns >
            <asp:TemplateField HeaderText="選択" HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate >
                    <asp:RadioButton ID="RadioButton1" name="RadioButton1" text="" runat="server"  />
                    </ItemTemplate>
         </asp:TemplateField>
          </Columns>
<HeaderStyle cssclass="fixdetail" Wrap="False" HorizontalAlign="Center"></HeaderStyle>
    <PagerStyle Width="100%" />
           <RowStyle Wrap="False" />
 </asp:GridView>
<asp:GridView cssclass="meisai_header" runat="server" ID="GridView2" Width="100%" >
        <Columns >
            <asp:TemplateField HeaderText="選択" HeaderStyle-HorizontalAlign="Center" >
                <ItemTemplate >
                    <asp:RadioButton ID="RadioButton1" runat="server"  />
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
        <HeaderStyle cssclass="fixdetail" Wrap="False"></HeaderStyle>
        <PagerStyle Width="100%" />
  </asp:GridView>
</asp:Panel>
<asp:Panel ID="Panel2" runat="server" HorizontalAlign="Center" Width="100%">
<div id="goukei_ran" class="goukei_ran">
    <table id="gokei_table" border="1">
        <tr><td style="background-color:#6699FF;width:100px">合計</td>
            <td id="goukeiran_keihi" style="width:150px;text-align:right;"></td>
            <td id="goukeiran_zei" style="width:150px;text-align:right;"></td>
            <td id="goukeiran_kin" style="width:150px;text-align:right;"></td>
        </tr>
    </table>
</div>
</asp:Panel>
<p>　　　</p>
<div>
<asp:button runat="server"  class="btn btn-default" id="Button_Disp" text="表示" style="height:50px"/>
<asp:button id="btnClose" class="btn btn-default"  runat="server" text="閉じる" style="height:50px"/>
</div>
 </div>      
<div id="dialog_trade" title="取引先マスタ">
<div>
<label>取引先</label><input type="text"id="d_torihki_name" class="input-sm" style="font-size:small  ;width:150px;margin-left:5px;margin-top:10px"/>
</div>
<div>
<label>カナ名</label><input type="text"id="d_kana_name" class="input-sm" style="font-size:small ;width:150px;margin-left:8px;margin-top:10px"/>
<input type="button" id="search_button_torihki" value="検索" class="btn btn-default" />
</div>
<div style="width:1000px; overflow-x: scroll;">
<table class="trade" id="trade" border="1">
</table>
</div>
</div>
<div id="dialog_msg" title="メッセージ">
<label id="msg" style="font-size:small;"></label> 
<input id="msg_status" type="hidden" />
</div>

<div id="msg_red" style="display:none; font-size:smaller ;color:red;"></div>
</div>

</asp:Content>
