<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Form_M_BUMON.aspx.vb" Inherits="KeihiWeb.Form_M_BUMON" %>
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

        $('#d_bumon_cd').css('ime-mode', 'inactive');
        $('#d_bumon_name').css('ime-mode', 'active');
        $('#d_saimu_cd').css('ime-mode', 'inactive');

        $('#d_bumon_cd').click(function () {
            $(this).select();
        });
        $('#d_bumon_name').click(function () {
            $(this).select();
        });
        $('#d_saimu_cd').click(function () {
            $(this).select();
        });

        //部門検索画面 
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
                    var bumoncd = document.getElementById("d_bumon_cd").value;
                    var bumonname = document.getElementById("d_bumon_name").value;
                    var saimucd = document.getElementById("d_saimu_cd").value;
                    var gyo = document.getElementById("d_gyo").value;
                    var kubun = document.getElementById("d_kubun").innerHTML;
                    var msg_red = document.getElementById("msg_red").innerHTML;

                    var JSONdata = {
                        bumon_cd: bumoncd,
                        bumon_name:bumonname,
                        saimu_cd:saimucd,
                        msg_red: msg_red
                    };

                    if (kubun === "新規モード") {
                        url_wk = 'Form_M_BUMON.aspx/InsertJOSNData';
                    }
                    if (kubun === "編集モード") {
                        url_wk = 'Form_M_BUMON.aspx/UpdateJOSNData';
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
                            var wk_msg = document.getElementById("msg");
                            //グリッドに更新値をセット
                            if (kubun === "編集モード") {
                                var tr = $("#MainContent_GridView1 tr");
                                var cells = tr.eq(gyo).children();
                                cells.eq(1).text(bumoncd);
                                cells.eq(2).text(bumonname);
                                cells.eq(3).text(saimucd);
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
        
        //削除確認ダイアログ
        var di_delete = $('#dialog_delete');
        //ダイアログを初期化（自動オープンしない）
        di_delete.dialog({
            modal: false,
            width: 250,
            height: 200,
            autoOpen: false,
            buttons: {
                '削除': function () {

                        var bumoncd = document.getElementById("d_bumon_cd").value;
                        var JSONdata = {
           　             bumon_cd:bumoncd
             　　       };

                 　　   url_wk = 'Form_M_BUMON.aspx/DeleteJOSNData';
                    　　$.ajax({
                        　 type: "POST",
                        　 url: url_wk,
                           data: JSON.stringify(JSONdata),
                           datatype: 'json',
                           contentType: 'application/json; charset=utf-8',
                           success: function (msg) {

                            var member = msg.d;
                            var result = member.result;
                           // var msglabel = document.getElementById("msglabel");
                            var wk_msg = document.getElementById("msg");
                            //グリッドに更新値をセット
                            //結果をメッセージボッツクスへ
                            wk_msg.innerHTML = member[0].msg;
                            if (msg_status.value === 'NG') {
                                di_msg.dialog('open');
                            }
                            return msg;
                          },
                           error: function (result, errorThrown) {

                               $('#msg').text('異常終了！！' + errorThrown);
                               di_msg.dialog('open');

                           }

                    }).done(function (data) {

                        window.location.reload();
                    
                    });

                },
                '閉じる': function () {
                    //window.location.reload();
                    $(this).dialog('close');

                }
            }
        });

  //*********下段３つのボタンの処理
        // 新規ボタンのイベントハンドラ
    $('#<%=newbumon.ClientID%>').click(function () {

            $('#<%=GridView1.ClientID%> input:radio').each(function () {
                $(this).prop('checked', false);
            });
            $('#d_bumon_cd').prop('disabled', false);
            
            var wk_BUMON_CD = document.getElementById("d_bumon_cd");
            var wk_BUMON_NAME = document.getElementById("d_bumon_name");
            var wk_SAIMU_CD = document.getElementById("d_saimu_cd");
            var wk_SAIMU_NAME = document.getElementById("d_saimu_name");
            wk_BUMON_CD.value = '';
            wk_BUMON_NAME.value = '';
            wk_SAIMU_CD.value = '';
            wk_SAIMU_NAME.innerHTML = '';

            di.dialog('open');

            var wk_kubun = document.getElementById("d_kubun");
            wk_kubun.innerHTML = "新規モード";
        
            return false;
        });

    //編集ボタンクリック
    $('#<%=editbumon.ClientID%>').click(function (e) {
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
                  
            var bumon_cd = cells.eq(1).text();//
            var bumon_name = cells.eq(2).text();//
            var saimu_cd = cells.eq(3).text();//
        
            //空白変換
            bumon_name= nbsp_func(bumon_name);
            
            //データをクリア
        
            di.dialog('open');

            var wk_BUMON_CD = document.getElementById("d_bumon_cd");
            var wk_BUMON_NAME = document.getElementById("d_bumon_name");
            var wk_SAIMU_CD = document.getElementById("d_saimu_cd");
            var wk_KUBUN = document.getElementById("d_kubun");
            var wk_GYO = document.getElementById("d_gyo");

            //部門名表示用関数
            //$('GridView1').func_bumon_nm(bumon_cd, wk_BUMON_NAME);
            //$('d_bumon_name').func_bumon_nm(bumon_cd, wk_BUMON_NAME);

            wk_BUMON_CD.value = bumon_cd;
            wk_BUMON_NAME.value = bumon_name;
            wk_SAIMU_CD.value = saimu_cd;
            wk_KUBUN.innerHTML = "編集モード";
            wk_GYO.value = gyo;

            $('#d_bumon_cd').prop('disabled', true);
            $('#d_bumon_name').focus;


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

            var bumon_cd = $(this).closest("tr").find("td").eq(1).text();
            var bumon_name = $(this).closest("tr").find("td").eq(2).text();
            var saimu_cd = $(this).closest("tr").find("td").eq(3).text();
        
            var wk_BUMON_CD = document.getElementById("d_bumon_cd");
            var wk_BUMON_NAME = document.getElementById("d_bumon_name");
            var wk_SAIMU_CD = document.getElementById("d_saimu_cd");
            var wk_SAIMU_NAME = document.getElementById("d_saimu_name");
            var wk_KUBUN = document.getElementById("d_kubun");
            var wk_GYO = document.getElementById("d_gyo");

            //部門名表示用関数
            //$('GridView1').func_bumon_nm(bumon_cd, wk_BUMON_NAME);
            //$('#d_bumon_name').func_bumon_nm(bumon_cd, wk_BUMON_NAME);
        //部門名表示用関数
           //$('GridView1').func_bumon_nm(bumon_cd, wk_BUMON_NAME);
            $('#d_saimu_name').func_bumon_nm(saimu_cd, wk_SAIMU_NAME);

            wk_BUMON_CD.value = bumon_cd;
            wk_BUMON_NAME.value = bumon_name;
            wk_SAIMU_CD.value = saimu_cd;
            wk_KUBUN.innerHTML = "編集モード";
            wk_GYO.value = row;
            $('#d_bumon_cd').prop('disabled', true);
            $('#d_bumon_name').focus;
       
        });

        //部門マスタ
        //行ダブルクリックで編集画面
        $("#MainContent_GridView1 td").dblclick(function () {
            //縦
            var row = $(this).closest('tr').index();
            //横
            var col = this.cellIndex;
            var bumon_cd= $(this).closest("tr").find("td").eq(1).text();
            var bumon_name = $(this).closest("tr").find("td").eq(2).text();
            var saimu_cd = $(this).closest("tr").find("td").eq(3).text();

            //空白変換
            bumon_name = nbsp_func(bumon_name);

            //編集画面オープン
            di.dialog('open');
      
            var wk_BUMON_CD = document.getElementById("d_bumon_cd");
            var wk_BUMON_NAME = document.getElementById("d_bumon_name");
            var wk_SAIMU_CD = document.getElementById("d_saimu_cd");
            var wk_SAIMU_NAME = document.getElementById("d_saimu_name");
            var wk_KUBUN = document.getElementById("d_kubun");
            var wk_GYO = document.getElementById("d_gyo");

            //部門名表示用関数
            //$('GridView1').func_bumon_nm(bumon_cd, wk_BUMON_NAME);
            $('d_saimu_name').func_bumon_nm(saimu_cd, wk_SAIMU_NAME);

            wk_BUMON_CD.value = bumon_cd;
            wk_BUMON_NAME.value = bumon_name;
            wk_SAIMU_CD.value = saimu_cd;
            wk_KUBUN.innerHTML  = "編集モード";
            wk_GYO.value = row;

            $('#d_bumon_id').prop('disabled', true);
            $('#d_bumon_name').focus;

            
        });

   
    
    // 行削除ボタンのイベントハンドラ
    $('#<%=delbumon.ClientID%>').click(function () {
        //wk_kubun.innerHTML = "削除モード";
        var wk_GYO = document.getElementById("d_gyo").value;

        if (wk_GYO.length === 0) {
            var msg = document.getElementById("msg");
            msg.innerHTML = '行選択してください';

            di_msg.dialog('open');
            return false
        }
  
        di_delete.dialog('open');
        return false;
    });

    $('#d_saimu_cd').on({
            change: function () {
                var msg_red = document.getElementById("msg_red");
                msg_red.innerHTML = '';

                var saimu_cd = document.getElementById("d_saimu_cd").value;

                var wk_SAIMU_NAME = document.getElementById("d_saimu_name");
                if (saimu_cd.length > 0) {
                    //部門名表示用関数
                    //$('#GridView1').func_bumon_nm(bumon_cd, wk_BUMON_NAME);
                    $('#d_saimu_name').func_bumon_nm(saimu_cd, wk_SAIMU_NAME);
                } else {
                    $('#d_saimu_name').text('');
                }

            }
        });
        //部門選択画面オープン
        $('#d_btn_serch').click(function (e) {

            var msg_red = document.getElementById("msg_red");
            msg_red.innerHTML = '';
            $('table#bumon *').remove();

            $.ajax({
                type: "POST",
                url: "Form_M_USER.aspx/GetJOSNData_Bumon",
                contentType: "application/json;charset=utf-8",
                data: {},
                dataType: "json",
                success: function (data) {

                    if (data.d.length > 0) {
                        $('#bumon').append("<tr style='background-color:#6699FF'><th>部門コード</th><th>部門名</th><th>債務部門</th></tr>");

                        for (var i = 0; i < data.d.length; i++) {
                            $('#bumon').append("<tr onClick='mClickTR(this)'><td>" + data.d[i].BUMON_CD + "</td> <td>" + data.d[i].BUMON_NM + "</td> <td>" + data.d[i].SAIMU_BMN + "</td></tr>");

                        }
                    }
                },
                error: function (result, errorThrown) {

                    $('#msg').text('異常終了！！' + errorThrown);
                    di_msg.dialog('open');

                }

            });
            di_bumon.dialog('open');

        });

    //
    //行クリック選択ラジオンボタンの解除とチェック
        //
     $('#bumon').click(function () {
            di_bumon.dialog('close');
     });


});
    //部門選択するイベント
    function mClickTR(obj) {

        var cd = document.getElementById('d_saimu_cd');
        var nm = document.getElementById('d_saimu_name');
        cd.value = obj.cells[0].innerHTML;
        nm.innerHTML = obj.cells[1].innerHTML;
        $(nm).css('color', 'black');
    }

</script>
    <div id="Container">
      <div class="index1">
    部門登録
      </div>
<asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" HorizontalAlign="Center" Width="100%"
        Height="400px" >
    <asp:GridView ID="GridView1" runat="server" HorizontalAlign="Center" Width="100%">
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
<asp:button runat="server"  class="btn btn-default" id="newbumon" text="新規" style="height:50px"/>
<asp:button runat="server"  class="btn btn-default" id="editbumon"  text="編集" style="height:50px"/>
<asp:button runat="server"  class="btn btn-default" id="delbumon" text="削除" style="height:50px"/>
<asp:button runat="server"  class="btn btn-default" id="closebumon" text="閉じる"  style="height:50px"/>
</div>
    
<div id="dialog_input" title="新規/編集">
<p>
<label id="d_kubun" style="color:red;">種別</label>
</p>
<div class="hi">
<input type="hidden" id="d_gyo" />
<table border ="0">
<tr>
<td>部門コード</td><td><input type="text"id="d_bumon_cd" class="input-sm" style="font-size:small;width:100px;"/></td>
</tr>
<tr>
<td>部門名</td><td><input type="text" id="d_bumon_name" class="input-sm" style="font-size:small;width:300px;" /></td>
</tr>
<tr>
<td>債務部門</td><td><input type="text" id="d_saimu_cd" class="input-sm" style="font-size:small;width:100px;"/><button id="d_btn_serch">検索</button></td>
</tr>
<tr>
<td>債務部門名</td><td><label id="d_saimu_name" style="height:10px ;font-size:smaller;color:blue" ></label></td>
</tr>
</table>
<p></p>
<div id="msg_red" style="display:none;font-size:smaller ;color:red;"></div>
</div>
</div>
<div id="dialog_msg" title="メッセージ">
<label id="msg" style="font-size:small;"></label> 
<input id="msg_status" type="hidden" />
</div>
<div id="dialog_bumon" title="部門マスタ">
<table id="bumon" border="1">
</table>
</div>
<div id="dialog_delete" title="メッセージ">
削除します　よろしいですか?
</div>
</div>    
</asp:Content>
