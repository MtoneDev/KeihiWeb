<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Form_M_KAMOKU.aspx.vb" Inherits="KeihiWeb.Form_M_KAMOKU" %>
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

        $('#d_kamoku_cd').css('ime-mode', 'inactive');
        $('#d_kamoku_name').css('ime-mode', 'active');
        
        $('#d_flg1').css('ime-mode', 'inactive');
        $('#d_flg2').css('ime-mode', 'inactive');
        $('#d_flg3').css('ime-mode', 'inactive');
        $('#d_tax_cd').css('ime-mode', 'inactive');
        $('#d_zei_cd').css('ime-mode', 'inactive');
        $('#d_disp_name').css('ime-mode', 'active');

        $('#d_kmoku_cd').click(function () {
            $(this).select();
        });
        $('#d_kamoku_name').click(function () {
            $(this).select();
        });
        $('#d_disp_name').click(function () {
            $(this).select();
        });
      
        //科目検索画面 
        var di_kamoku = $('#dialog_kamoku');
        di_kamoku.dialog({
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
                    var kamokucd = document.getElementById("d_kamoku_cd").value;
                    var kamokuname = document.getElementById("d_kamoku_name").value;
                    var dispname = document.getElementById("d_disp_name").value;
                    var flg1= document.getElementById("d_flg1").value;
                    var flg2= document.getElementById("d_flg2").value;
                    var flg3= document.getElementById("d_flg3").value;
                    var taxcd= document.getElementById("d_tax_cd").value;
                    var zeicd= document.getElementById("d_zei_cd").value;
                    
                    var gyo = document.getElementById("d_gyo").value;
                    var kubun = document.getElementById("d_kubun").innerHTML;
                    var msg_red = document.getElementById("msg_red").innerHTML;

                    var JSONdata = {
                        kamoku_cd: kamokucd,
                        kamoku_name: kamokuname,
                        disp_name: dispname,
                        flg1: flg1,
                        flg2: flg2,
                        flg3: flg3,
                        tax_cd: taxcd,
                        zei_cd: zeicd,
                        msg_red: msg_red
                    };

                    if (kubun === "新規モード") {
                        url_wk = 'Form_M_KAMOKU.aspx/InsertJOSNData';
                    }
                    if (kubun === "編集モード") {
                        url_wk = 'Form_M_KAMOKU.aspx/UpdateJOSNData';
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
                                cells.eq(1).text(kamokucd);
                                cells.eq(2).text(kamokuname);
                                cells.eq(3).text(dispname);
                                if (flg1 === '1') {
                                    $('#<%=GridView1.ClientID%> tr:eq(' + gyo + ') td:eq(4) [type="checkbox"]').prop('checked', true);
                                } else {
                                    $('#<%=GridView1.ClientID%> tr:eq(' + gyo + ') td:eq(4) [type="checkbox"]').prop('checked', false);
                                }
                                if (flg2 === '1') {
                                    $('#<%=GridView1.ClientID%> tr:eq(' + gyo + ') td:eq(5) [type="checkbox"]').prop('checked', true);
                                } else {
                                    $('#<%=GridView1.ClientID%> tr:eq(' + gyo + ') td:eq(5) [type="checkbox"]').prop('checked', false);
                                }
                                if (flg3 === '1') {
                                    $('#<%=GridView1.ClientID%> tr:eq(' + gyo + ') td:eq(6) [type="checkbox"]').prop('checked', true);
                                } else {
                                    $('#<%=GridView1.ClientID%> tr:eq(' + gyo + ') td:eq(6) [type="checkbox"]').prop('checked', false);
                                }
                                cells.eq(7).text(taxcd);
                                cells.eq(8).text(zeicd);

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

                        var kamokucd = document.getElementById("d_kamoku_cd").value;
                        var JSONdata = {
           　             kamoku_cd:kamokucd
             　　       };

                 　　   url_wk = 'Form_M_KAMOKU.aspx/DeleteJOSNData';
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
    $('#<%=newkamoku.ClientID%>').click(function () {

            $('#<%=GridView1.ClientID%> input:radio').each(function () {
                $(this).prop('checked', false);
            });
            $('#d_kamoku_cd').prop('disabled', false);
            
            var wk_KAMOKU_CD = document.getElementById("d_kamoku_cd");
            var wk_KAMOKU_NAME = document.getElementById("d_kamoku_name");
            var wk_DISP_NAME = document.getElementById("d_disp_name");
            var wk_FLG1 = document.getElementById("d_flg1");
            var wk_FLG2 = document.getElementById("d_flg2");
            var wk_FLG3 = document.getElementById("d_flg3");
            var wk_TAX_CD = document.getElementById("d_tax_cd");
            var wk_ZEI_CD = document.getElementById("d_zei_cd");
            wk_KAMOKU_CD.value = '';
            wk_KAMOKU_NAME.value = '';
            wk_DISP_NAME.value = '';
            wk_FLG1.value= '0';
            wk_FLG2.value = '0';
            wk_FLG3.value = '0';
            wk_TAX_CD.value = '';
            wk_ZEI_CD.value = '';

            di.dialog('open');

            var wk_kubun = document.getElementById("d_kubun");
            wk_kubun.innerHTML = "新規モード";
        
            return false;
        });

    //編集ボタンクリック
    $('#<%=editkamoku.ClientID%>').click(function (e) {
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
            var wkflg1;
            var wkflg2;
            var wkflg3;

    
            if ($('#<%=GridView1.ClientID%> tr:eq(' + gyo + ') td:eq(4) [type="checkbox"]').prop('checked')){
                wkflg1 = '1';
            } else {
                wkflg1 = '0';
            }

            if ($('#<%=GridView1.ClientID%> tr:eq(' + gyo + ') td:eq(5) [type="checkbox"]').prop('checked')) {
                wkflg2 = '1';
            } else {
                wkflg2 = '0';
            }

            if ($('#<%=GridView1.ClientID%> tr:eq(' + gyo + ') td:eq(6) [type="checkbox"]').prop('checked')) {
                wkflg3 = '1';
            } else {
                wkflg3 = '0';
            }
                     
            var kamoku_cd = cells.eq(1).text();//
            var kamoku_name = cells.eq(2).text();//
            var disp_name = cells.eq(3).text();//
            var flg1 = wkflg1;//
            var flg2 = wkflg2;//
            var flg3 = wkflg3;//
            var tax_cd = cells.eq(7).text();//
            var zei_cd = cells.eq(8).text();//

            //空白変換
            kamoku_name= nbsp_func(kamoku_name);
            disp_name = nbsp_func(disp_name);
            
            //データをクリア
        
            di.dialog('open');

            var wk_KAMOKU_CD = document.getElementById("d_kamoku_cd");
            var wk_KAMOKU_NAME = document.getElementById("d_kamoku_name");
            var wk_DISP_NAME = document.getElementById("d_disp_name");
            var wk_FLG1 = document.getElementById("d_flg1");
            var wk_FLG2 = document.getElementById("d_flg2");
            var wk_FLG3 = document.getElementById("d_flg3");
            var wk_TAX_CD = document.getElementById("d_tax_cd");
            var wk_ZEI_CD = document.getElementById("d_zei_cd");

            var wk_KUBUN = document.getElementById("d_kubun");
            var wk_GYO = document.getElementById("d_gyo");

            //$('GridView1').func_bumon_nm(bumon_cd, wk_BUMON_NAME);
            //$('d_bumon_name').func_bumon_nm(bumon_cd, wk_BUMON_NAME);
            wk_KAMOKU_CD.value = kamoku_cd;
            wk_KAMOKU_NAME.value = kamoku_name;
            wk_DISP_NAME.value = disp_name;
            wk_FLG1.value = flg1;
            wk_FLG2.value = flg2;
            wk_FLG3.value = flg3;
            wk_TAX_CD.value = tax_cd;
            wk_ZEI_CD.value = zei_cd;

            wk_KUBUN.innerHTML = "編集モード";
            wk_GYO.value = gyo;

            $('#d_kamoku_cd').prop('disabled', true);
            $('#d_kamoku_name').focus;


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

   
            var kamoku_cd = $(this).closest("tr").find("td").eq(1).text();
            var kamoku_name = $(this).closest("tr").find("td").eq(2).text();
            var disp_name = $(this).closest("tr").find("td").eq(3).text();

            var tax_cd = $(this).closest("tr").find("td").eq(7).text();
            var zei_cd = $(this).closest("tr").find("td").eq(8).text();


            var wk_KAMOKU_CD = document.getElementById("d_kamoku_cd");
            var wk_KAMOKU_NAME = document.getElementById("d_kamoku_name");
            var wk_DISP_NAME = document.getElementById("d_disp_name");
            var wk_TAX_CD = document.getElementById("d_tax_cd");
            var wk_ZEI_CD = document.getElementById("d_zei_cd");

            var wk_KUBUN = document.getElementById("d_kubun");
            var wk_GYO = document.getElementById("d_gyo");


            wk_KAMOKU_CD.value = kamoku_cd;
            wk_KAMOKU_NAME.value = kamoku_name;
            wk_DISP_NAME.value = disp_name;
            wk_TAX_CD.value = tax_cd;
            wk_ZEI_CD.value = zei_cd;

            wk_KUBUN.innerHTML = "編集モード";
            wk_GYO.value = row;
            $('#d_kamoku_cd').prop('disabled', true);
            $('#d_kamoku_name').focus;
       
        });

        //科目マスタ
        //行ダブルクリックで編集画面
        $("#MainContent_GridView1 td").dblclick(function () {
            //縦
            var row = $(this).closest('tr').index();
            //横
            var col = this.cellIndex;
            var kamoku_cd= $(this).closest("tr").find("td").eq(1).text();
            var kamoku_name = $(this).closest("tr").find("td").eq(2).text();
            var disp_name = $(this).closest("tr").find("td").eq(3).text();

            var wkflg1;
            var wkflg2;
            var wkflg3;

    
            if ($('#<%=GridView1.ClientID%> tr:eq(' + row + ') td:eq(4) [type="checkbox"]').prop('checked')){
                wkflg1 = '1';
            } else {
                wkflg1 = '0';
            }

            if ($('#<%=GridView1.ClientID%> tr:eq(' + row + ') td:eq(5) [type="checkbox"]').prop('checked')) {
                wkflg2 = '1';
            } else {
                wkflg2 = '0';
            }

            if ($('#<%=GridView1.ClientID%> tr:eq(' + row + ') td:eq(6) [type="checkbox"]').prop('checked')) {
                wkflg3 = '1';
            } else {
                wkflg3 = '0';
            }
           
            var flg1 = wkflg1;
            var flg2 = wkflg2;
            var flg3 = wkflg3;
            var tax_cd = $(this).closest("tr").find("td").eq(7).text();
            var zei_cd = $(this).closest("tr").find("td").eq(8).text();

            //空白変換
            kamoku_name = nbsp_func(kamoku_name);
            disp_name = nbsp_func(disp_name);
            flg1 = nbsp_func(flg1);
            flg2 = nbsp_func(flg2);
            flg3 = nbsp_func(flg3);


            //編集画面オープン
            di.dialog('open');
      
            var wk_KAMOKU_CD = document.getElementById("d_kamoku_cd");
            var wk_KAMOKU_NAME = document.getElementById("d_kamoku_name");
            var wk_DISP_NAME = document.getElementById("d_disp_name");
            var wk_FLG1 = document.getElementById("d_flg1");
            var wk_FLG2 = document.getElementById("d_flg2");
            var wk_FLG3 = document.getElementById("d_flg3");
            var wk_TAX_CD = document.getElementById("d_tax_cd");
            var wk_ZEI_CD = document.getElementById("d_zei_cd");

            var wk_KUBUN = document.getElementById("d_kubun");
            var wk_GYO = document.getElementById("d_gyo");

         
            wk_KAMOKU_CD.value = kamoku_cd;
            wk_KAMOKU_NAME.value = kamoku_name;
            wk_DISP_NAME.value = disp_name;
            wk_FLG1.value = flg1;
            wk_FLG2.value = flg2;
            wk_FLG3.value = flg3;
            wk_TAX_CD.value = tax_cd;
            wk_ZEI_CD.value = zei_cd;

            wk_KUBUN.innerHTML = "編集モード";
            wk_GYO.value = row;

            $('#d_kamoku_cd').prop('disabled', true);
            $('#d_kamoku_name').focus;

            
        });

   
    
    // 行削除ボタンのイベントハンドラ
    $('#<%=delkamoku.ClientID%>').click(function () {
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

     
    //
    //行クリック選択ラジオンボタンの解除とチェック
        //
     $('#kamoku').click(function () {
            di_kamoku.dialog('close');
     });


});
   
</script>
    <div id="Container">
      <div class="index1">
        科目登録
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
<asp:button runat="server"  class="btn btn-default" id="newkamoku" text="新規" style="height:50px"/>
<asp:button runat="server"  class="btn btn-default" id="editkamoku"  text="編集" style="height:50px"/>
<asp:button runat="server"  class="btn btn-default" id="delkamoku" text="削除" style="height:50px"/>
<asp:button runat="server"  class="btn btn-default" id="closekamoku" text="閉じる"  style="height:50px"/>
</div>
    
<div id="dialog_input" title="新規/編集">
<p>
<label id="d_kubun" style="color:red;">種別</label>
</p>
<div class="hi">
<input type="hidden" id="d_gyo" />
<table border ="0">
<tr>
<td>科目コード</td><td><input type="text"id="d_kamoku_cd" class="input-sm" style="font-size:small;width:100px;"/></td>
</tr>
<tr>
<td>科目名</td><td><input type="text" id="d_kamoku_name" class="input-sm" style="font-size:small;width:300px;" /></td>
</tr>
<tr>
<td>表示名</td><td><input type="text" id="d_disp_name" class="input-sm" style="font-size:small;width:200px;"/></td>
</tr>
<tr>
<td>FLG1</td><td><input type="text" id="d_flg1" class="input-sm" style="font-size:small;width:50px;"/>(0,1)</td>
</tr>
<tr>
<td>FLG2</td><td><input type="text" id="d_flg2" class="input-sm" style="font-size:small;width:50px;"/>(0,1)</td>
</tr>
<tr>
<td>FLG3</td><td><input type="text" id="d_flg3" class="input-sm" style="font-size:small;width:50px;"/>(0,1)</td>
</tr>
<tr>
<td>税区分</td><td><input type="text" id="d_tax_cd" class="input-sm" style="font-size:small;width:100px;"/>(1:内税,2:外税,3:非課税,4:その他)</td>
</tr>
<tr>
<td>消費税区分</td><td><input type="text" id="d_zei_cd" class="input-sm" style="font-size:small;width:100px;"/>(0,1)</td>
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
<div id="dialog_kamoku" title="科目マスタ">
<table id="kamoku" border="1">
</table>
</div>
<div id="dialog_delete" title="メッセージ">
削除します　よろしいですか?
</div>
</div>    
</asp:Content>
