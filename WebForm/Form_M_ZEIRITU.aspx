<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Form_M_ZEIRITU.aspx.vb" Inherits="KeihiWeb.Form_M_ZEIRITU" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<style type="text/css">
     /* --- マウスオーバー時 --- */
table#bumon tr:hover {
background-color: #CC99FF; /* 行の背景色 */
}
</style>
<script type="text/javascript" src="../Scripts/WebForms/STSCommon/common.js" ></script>
<script type="text/javascript" src="../Scripts/WebForms/STSCommon/keihi_user.js" ></script>
<script type="text/javascript" src="../Scripts/WebForms/STSCommon/keihi_bumon.js" ></script>

<script type="text/javascript">

    $(function () {
        var errchk_date1;
        var errchk_date2;


        $('#d_bunrui').css('ime-mode', 'inactive');
        $('#d_zeiritu').css('ime-mode', 'inactive');
        $('#d_start_date').css('ime-mode', 'inactive');
        $('#d_end_date').css('ime-mode', 'inactive');
  

        $('#d_bunrui').focus(function () {
            $(this).select();
        });

        $('#d_zeiritu').focus(function () {
            $(this).select();
        });

        $('#d_start_date').focus(function () {
            $(this).select();
        });
        $('#d_end_date').focus(function () {
            $(this).select();
        });


        //datepiclker
        $.datepicker.setDefaults($.datepicker.regional['ja']);

        $('#d_start_date').datepicker({
            dateFormat: 'yy/mm/dd',
            onSelect: function (date, obj) {
                $('#d_zeiritu').focus();
            },
        });

        $('#d_end_date').datepicker({
            dateFormat: 'yy/mm/dd',
            onSelect: function (date, obj) {
                $('#dialog_input').siblings('.ui-dialog-buttonpane').find('button:eq(0)').focus();
            },
        });
   
           //購入日を変更したら更新ボタンをＦＡＬＳＥ
        $('#d_start_date').on({
            change: function () {
                errchk_date1 = '';

                if ($('#d_start_date').val().length > 10) {

                    $('#msg').text('開始入力日の値が不正です(yyyy/MM/dd)');
                    $('#err_msg_lbl').val('開始入力日エラー');
                    errchk_date1 = '1';
                    di_msg.dialog('open');
                    return false;
                } else {
                    $('#err_msg_lbl').val('');
                }

                var datestr = $('#d_start_date').val();

                if (datestr.match(/[0-9]{8}/)) {
                    str1 = datestr.substring(0, 4) + "/" + datestr.substring(4, 6) + "/" + datestr.substring(6, 8)
                    $('#d_start_date').val(str1);
                    datestr = $('#d_start_date').val();
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
                        $('#msg').text('開始入力日の値が不正です(yyyy/MM/dd)');
                        errchk_date1 = '1';
                        $('#err_msg_lbl').val('開始入力日エラー');
                        di_msg.dialog('open');
                        return false;
                    }
                } else {
                    $('#err_msg_lbl').val('開始入力日エラー');
                    errchk_date1 = '1';
                    $('#msg').text('開始入力日の値が不正です(yyyy/MM/dd)');
                    di_msg.dialog('open');

                    return false;
                }

                if ($('#d_start_date').val().length < 10) {
                    errchk_date1 = '1';
                    $('#msg').text('開始入力日の値が不正です(yyyy/MM/dd)');
                    $('#err_msg_lbl').val('開始入力日エラー');
                    di_msg.dialog('open');
                    return false;
                } else {
                    $('#err_msg_lbl').val('');

                }


            }
        });

    $('#d_end_date').on({
        change: function () {
           errchk_date2 = '';

           if ($('#d_end_date').val().length > 10) {
               errchk_date2 = '1';
               $('#msg').text('終了入力日の値が不正です(yyyy/MM/dd)');
                $('#err_msg_lbl').val('終了入力日エラー');
                di_msg.dialog('open');
                return false;
            }

           var datestr = $('#d_end_date').val();

            if (datestr.match(/[0-9]{8}/)) {
                str1 = datestr.substring(0, 4) + "/" + datestr.substring(4, 6) + "/" + datestr.substring(6, 8)
                $('#d_end_date').val(str1);
                datestr = $('#d_end_date').val();
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
                    $('#msg').text('終了入力日の値が不正です(yyyy/MM/dd)');
                    errchk_date2 = '1';
                    $('#err_msg_lbl').val('終了入力日エラー');
                    di_msg.dialog('open');
                    return false;
                } else if (vDt.getFullYear() == vYear
                 && vDt.getMonth() == vMonth
                 && vDt.getDate() == vDay) {
                } else {
                    errchk_date2 = '1';
                    $('#msg').text('終了入力日の値が不正です(yyyy/MM/dd)');
                    $('#err_msg_lbl').val('終了入力日エラー');
                    di_msg.dialog('open');
                    return false;
                }
            } else {
                errchk_date2 = '1';
                $('#err_msg_lbl').val('終了入力日エラー');
                $('#msg').text('終了入力日の値が不正です(yyyy/MM/dd)');
                di_msg.dialog('open');

                return false;
            }

            if ($('#d_end_date').val().length < 10) {
                 errchk_date2 = '1';
                 $('#msg').text('終了入力日の値が不正です(yyyy/MM/dd)');
                 $('#err_msg_lbl').val('終了入力日エラー');
                 di_msg.dialog('open');
                 return false;
            }

         
        }
    });


        //登録画面
        var di = $('#dialog_input');
        //ダイアログを初期化（自動オープンしない）
        di.dialog({
            modal: true,
            width: 500,
            height: 430,
            autoOpen: false,
            buttons: {

                '更新': function () {

                    if (errchk_date1 === '1' || errchk_date2 === '1') {


                        $('#msg').text('エラーが存在します');
                        di_msg.dialog('open');

                        return false

                    }

                    var bunrui = document.getElementById("d_bunrui").value;
                    var startdate = document.getElementById("d_start_date").value;
                    var zeiritu = document.getElementById("d_zeiritu").value;
                    var enddate = document.getElementById("d_end_date").value;
                    var gyo = document.getElementById("d_gyo").value;
                    var kubun = document.getElementById("d_kubun").innerHTML;
                    var msg_red = document.getElementById("msg_red").innerHTML;


                    if (kubun === "新規モード") {
                        url_wk = 'Form_M_ZEIRITU.aspx/InsertJOSNData';
                        var JSONdata = {
                            bunrui: bunrui,
                            startdate: startdate,
                            zeiritu: zeiritu,
                            enddate: enddate,
                            bunrui_old: '',
                            startdate_old: '',
                            enddate_old: '',
                            msg_red: msg_red
                        };


                    }
                    if (kubun === "編集モード") {
                        url_wk = 'Form_M_ZEIRITU.aspx/UpdateJOSNData';
                        var JSONdata = {
                            bunrui: bunrui,
                            startdate: startdate,
                            zeiritu: zeiritu,
                            enddate: enddate,
                            bunrui_old: $('#<%=old_bunrui.ClientID %>').val(),
                            startdate_old: $('#<%=old_startdate.ClientID %>').val(),
                            enddate_old: $('#<%=old_enddate.ClientID %>').val(),
                            msg_red: msg_red
                        };

                    }

                    $.ajax({
                        type: "POST",
                        url: url_wk,
                        data: JSON.stringify(JSONdata),
                        datatype: 'json',
                        contentType: 'application/json; charset=utf-8',
                        success: function (msg) {

                            var msg_status = document.getElementById("msg_status");
                            var member = msg.d;
                            var result = member.result;
                            // var msglabel = document.getElementById("msglabel");
                            var wk_msg = document.getElementById("msg");
                            //グリッドに更新値をセット
                            if (kubun === "編集モード") {
                                var tr = $("#MainContent_GridView1 tr");
                                var cells = tr.eq(gyo).children();
                                cells.eq(1).text(bunrui);
                                cells.eq(2).text(startdate);
                                cells.eq(3).text(zeiritu);
                                cells.eq(4).text(enddate);
                            }
                            //結果をメッセージボッツクスへ
                            wk_msg.innerHTML = member[0].msg;
                            msg_status.value = member[0].result;
                            console.log(member[0].result);
                            if (msg_status.value === 'NG') {
                                di_msg.dialog('open');
                            }
                            return msg;
                        },
                        error: function (data, errorThrown) {
                            $('#msg').text('異常終了！！' + errorThrown);
                            di_msg.dialog('open');

                        }
                    }).done(function (data) {
                        var msg_status = document.getElementById("msg_status");
                        if (msg_status.value === 'OK') {
                            window.location.reload();
                        }
                    });

                },
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
        
 
  //*********下段３つのボタンの処理
        // 新規ボタンのイベントハンドラ
    $('#<%=newzei.ClientID%>').click(function () {

            $('#<%=GridView1.ClientID%> input:radio').each(function () {
                $(this).prop('checked', false);
            });
            $('#d_user_id').prop('disabled', false);
            
            var wk_BUNRUI = document.getElementById("d_bunrui");
            var wk_ZEIRITU = document.getElementById("d_zeiritu");
            var wk_START_DATE = document.getElementById("d_start_date");
            var wk_END_DATE = document.getElementById("d_end_date");
            var wk_KUBUN = document.getElementById("d_kubun");
            wk_BUNRUI.value = '';
            wk_ZEIRITU.value = '';
            wk_START_DATE.value = '';
            wk_END_DATE.value = '';
            wk_KUBUN.innerHTML = '';
           
            di.dialog('open');

            var wk_kubun = document.getElementById("d_kubun");
            wk_kubun.innerHTML = "新規モード";
        
            return false;
     });

    //編集ボタンクリック
    $('#<%=editzei.ClientID%>').click(function (e) {
        var chk = '0';
        $('#<%=GridView1.ClientID%> input:radio').each(function () {
           if ($(this).prop('checked') === true) {

                chk = '1';
                  
           }
         });

        if (chk === '1') {
            var wk_GYO = document.getElementById("d_gyo");
            var gyo = wk_GYO.value;
            var tr = $('#<%=GridView1.ClientID%> tr');
            var cells = tr.eq(gyo).children();
                  
            var bunrui = cells.eq(1).text();//
            var zeiritu = cells.eq(3).text();//
            var startdate = cells.eq(2).text();//
            var enddate = cells.eq(4).text();//
      
            //空白変換
            bunrui = nbsp_func(bunrui);

            //データをクリア
        
            di.dialog('open');

            var wk_BUNRUI = document.getElementById("d_bunrui");
            var wk_ZEIRITU = document.getElementById("d_zeiritu");
            var wk_START_DATE = document.getElementById("d_start_date");
            var wk_END_DATE = document.getElementById("d_end_date");
            var wk_KUBUN = document.getElementById("d_kubun");
            var wk_GYO = document.getElementById("d_gyo");

           
            wk_BUNRUI.value = bunrui;
            wk_ZEIRITU.value = zeiritu;
            wk_START_DATE.value = startdate;
            wk_END_DATE.value = enddate;
            wk_KUBUN.innerHTML = "編集モード";
            wk_GYO.value = gyo;

            //編集前の値をキープ
            $('#<%=old_bunrui.ClientID%>').val(bunrui);
            $('#<%=old_startdate.ClientID%>').val(startdate);
            $('#<%=old_enddate.ClientID%>').val(enddate);

            $('#d_zeiritu').focus;


        } else {

            $('#msg').text('行選択されていません');
            di_msg.dialog('open');
            return false;

        }
        return false;

    });


        //行クリックでラジオON
    $("#MainContent_GridView1 td").click(function () {
            //縦
            var row = $(this).closest('tr').index();
            //横
            var col = this.cellIndex;
  
            $('#<%=GridView1.ClientID%> input:radio').each(function () {
                $(this).prop('checked', false);
            });
            $(this).closest("tr").find("td input:radio").each(function () {
               $(this).prop('checked', true);
            });

            var bunrui = $(this).closest("tr").find("td").eq(1).text();
            var zeiritu = $(this).closest("tr").find("td").eq(3).text();
            var startdate = $(this).closest("tr").find("td").eq(2).text();
            var enddate = $(this).closest("tr").find("td").eq(4).text();
        
            var wk_BUNRUI = document.getElementById("d_bunrui");
            var wk_ZEIRITU = document.getElementById("d_zeiritu");
            var wk_START_DATE = document.getElementById("d_start_date");
            var wk_END_DATE = document.getElementById("d_end_date");
            var wk_KUBUN = document.getElementById("d_kubun");
            var wk_GYO = document.getElementById("d_gyo");

        
            wk_BUNRUI.value = bunrui;
            wk_ZEIRITU.value = zeiritu;
            wk_START_DATE.value = startdate;
            wk_END_DATE.value = enddate;
            wk_KUBUN.innerHTML = "編集モード";
            wk_GYO.value = row;

       
     });


        //ユーザーマスタ
        //行ダブルクリックで編集画面
    $("#MainContent_GridView1 td").dblclick(function () {
            //縦
            var row = $(this).closest('tr').index();
            //横
            var col = this.cellIndex;
            var bunrui= $(this).closest("tr").find("td").eq(1).text();
            var zeiritu = $(this).closest("tr").find("td").eq(3).text();
            var startdate = $(this).closest("tr").find("td").eq(2).text();
            var enddate = $(this).closest("tr").find("td").eq(4).text();
      
            //空白変換
            bunrui = nbsp_func(bunrui);
      
            //編集画面オープン
            di.dialog('open');
      
            var wk_BUNRUI = document.getElementById("d_bunrui");
            var wk_ZEIRITU = document.getElementById("d_zeiritu");
            var wk_START_DATE = document.getElementById("d_start_date");
            var wk_END_DATE = document.getElementById("d_end_date");
            var wk_KUBUN = document.getElementById("d_kubun");
            var wk_GYO = document.getElementById("d_gyo");

      
            wk_BUNRUI.value = bunrui;
            wk_ZEIRITU.value = zeiritu;
            wk_START_DATE.value = startdate;
            wk_END_DATE.value = enddate;
            wk_KUBUN.innerHTML  = "編集モード";
            wk_GYO.value = row;

             //編集前の値をキープ
            $('#<%=old_bunrui.ClientID%>').val(bunrui);
            $('#<%=old_startdate.ClientID%>').val(startdate);
            $('#<%=old_enddate.ClientID%>').val(enddate);

            
        });


        
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

    });
   
</script>
    <div id="Container">
      <div class="index1">
    消費税登録
        </div>
<asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" HorizontalAlign="Center" Width="100%"
        Height="280px" >
        <asp:GridView ID="GridView1" cssclass="mastertable td" runat="server" HorizontalAlign="Center" Width="100%">
         <Columns>
            <asp:TemplateField HeaderText="選択" HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate >
                    <asp:RadioButton ID="RadioButton1" runat="server"  />
                    </ItemTemplate>
         </asp:TemplateField>
          </Columns>
        <HeaderStyle cssclass="fixdetail" Wrap="False" BackColor="#99CCFF" />
    </asp:GridView>
    </asp:Panel>
  
<div style="margin-top:5px">
<asp:button runat="server"  class="btn btn-default" id="newzei" text="新規" style="height:50px"/>
<asp:button runat="server"  class="btn btn-default" id="editzei"  text="編集" style="height:50px"/>
<asp:button runat="server"  class="hidden" id="deluser" text="削除" style="height:50px"/>
<asp:button runat="server"  class="btn btn-default" id="closezei" text="閉じる"  style="height:50px"/>
</div>
    
<div id="dialog_input" title="新規/編集">
<p>
<label id="d_kubun" style="color:red;">種別</label>
</p>
<div class="hi">
<input type="hidden" id="d_gyo" />
<table border ="0">
<tr>
<td>分類</td><td><input type="text"id="d_bunrui" class="input-sm" style="font-size:small;width:100px;"/></td>
</tr>
<tr>
<td>開始日</td><td><input type="text" id="d_start_date" class="input-sm" style="font-size:small;width:150px;"/>
</tr>
<tr>
<td>税率</td><td><input type="text" id="d_zeiritu" class="input-sm" style="font-size:small;width:150px;" />
</tr>
<tr> 
<td>終了日</td><td><input type="text" id="d_end_date" class="input-sm" style="font-size:small;width:150px;"/>
</tr>
</table>
<input type="text" id="d_work" class="hidden"/>
<p></p>
<div id="msg_red" style="display:none;font-size:smaller ;color:red;"></div>
</div>
</div>
<div class="hidden">
<asp:TextBox id ="old_bunrui" runat="server"></asp:TextBox> 
<asp:TextBox id ="old_startdate" runat="server"></asp:TextBox> 
<asp:TextBox id ="old_enddate" runat="server"></asp:TextBox> 
</div>
<div id="dialog_msg" title="メッセージ">
<label id="msg" style="font-size:small;"></label> 
<input id="msg_status" type="hidden" />
</div>
 </div>    

</asp:Content>
