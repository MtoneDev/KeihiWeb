<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Form_M_TRADE.aspx.vb" Inherits="KeihiWeb.Form_M_TRADE" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<style type="text/css">
     /* --- マウスオーバー時 --- */
table#bumon tr:hover {
background-color: #CC99FF; /* 行の背景色 */
}
</style>
<script type="text/javascript">
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

    $(function () {

    
        $('#d_trade_cd').css('ime-mode', 'inactive');
        $('#d_trade_name').css('ime-mode', 'active');
        $('#d_trade_kana').css('ime-mode', 'active');
       
        $('#d_trade_cd').click(function () {
            $(this).select();
        });
        $('#d_trade_name').click(function () {
            $(this).select();
        });
        $('#d_trade_kana').click(function () {
            $(this).select();
        });

    
        //登録画面
        var di = $('#dialog_input');
        //ダイアログを初期化（自動オープンしない）
        di.dialog({
            modal: true,
            width: 500,
            height: 600,
            autoOpen: false,
            buttons: {
                
                '更新': function () {
                  
                    var tradecd = document.getElementById("d_trade_cd").value;
                    var tradename = document.getElementById("d_trade_name").value;
                    var tradekana = document.getElementById("d_trade_kana").value;
                    var address1 = document.getElementById("d_address1").value;
                    var address2 = document.getElementById("d_address2").value;
                    var bankcd = document.getElementById("d_bank_cd").value;
                    var branchcd = document.getElementById("d_branch_cd").value;
                    var acccd = document.getElementById("d_acc_cd").value;
                    var accno = document.getElementById("d_acc_no").value;
                    var accnm = document.getElementById("d_acc_nm").value;
                    var mibaraicd = document.getElementById("d_mibarai_cd").value;
                    var shiharaicd = document.getElementById("d_shiharai_cd").value;
                    var keiyakuno = document.getElementById("d_keiyaku_no").value;

                    var gyo = document.getElementById("d_gyo").value;
                    var kubun = document.getElementById("d_kubun").innerHTML;
                    var msg_red = document.getElementById("msg_red").innerHTML;
                  
                    var JSONdata = {
                        test :[
                            tradecd,
                            tradename,
                            tradekana,
                            address1,
                            address2,
                            bankcd,
                            branchcd,
                            acccd,
                            accno,
                            accnm,
                            mibaraicd,
                            shiharaicd,
                            keiyakuno,
                            msg_red
                        ]
                    };

                    if (kubun === "新規モード") {
                        url_wk = 'Form_M_TRADE.aspx/InsertJOSNData';
                    }
                    if (kubun === "編集モード") {
                        url_wk = 'Form_M_TRADE.aspx/UpdateJOSNData';
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
                                cells.eq(1).text(tradecd);
                                cells.eq(2).text(tradename);
                                cells.eq(3).text(tradekana);
                                cells.eq(4).text(address1);
                                cells.eq(5).text(address2);
                                cells.eq(6).text(bankcd);
                                cells.eq(7).text(branchcd);
                                cells.eq(8).text(acccd);
                                cells.eq(9).text(accno);
                                cells.eq(10).text(accnm);
                                cells.eq(11).text(mibaraicd);
                                cells.eq(12).text(shiharaicd);
                                cells.eq(13).text(keiyakuno);
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

                    var tradecd = document.getElementById("d_trade_cd").value;
                    var JSONdata = {
                          trade_cd:tradecd
             　　       };

                 　　   url_wk = 'Form_M_TRADE.aspx/DeleteJOSNData';
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
    $('#<%=newtrade.ClientID%>').click(function () {

            $('#<%=GridView1.ClientID%> input:radio').each(function () {
                $(this).prop('checked', false);
            });
            $('#d_trade_cd').prop('disabled', false);
            
            var wk_tradecd = document.getElementById("d_trade_cd").value;
            var wk_tradename = document.getElementById("d_trade_name").value;
            var wk_tardekana = document.getElementById("d_trade_kana").value;
            var wk_address1 = document.getElementById("d_address1").value;
            var wk_address2 = document.getElementById("d_address2").value;
            var wk_bankcd = document.getElementById("d_bank_cd").value;
            var wk_branchcd = document.getElementById("d_branch_cd").value;
            var wk_acccd = document.getElementById("d_acc_cd").value;
            var wk_accno = document.getElementById("d_acc_no").value;
            var wk_accnm = document.getElementById("d_acc_nm").value;
            var wk_mibaraicd = document.getElementById("d_mibarai_cd").value;
            var wk_shiharaicd = document.getElementById("d_shiharai_cd").value;
            var wk_keiyakuno = document.getElementById("d_keiyaku_no").value;


            wk_tradecd = '';
            wk_tradename = '';
            wk_tardekana = ''; 
            wk_address1 = ''; 
            wk_address2 = '';
            wk_bankcd = '';
            wk_branchcd = ''; 
            wk_acccd = '';
            wk_accno = ''; 
            wk_accnm = ''; 
            wk_mibaraicd = ''; 
            wk_shiharaicd = '';
            wk_keiyakuno = ''; 

            di.dialog('open');

            var wk_kubun = document.getElementById("d_kubun");
            wk_kubun.innerHTML = "新規モード";
        
            return false;
        });

    //編集ボタンクリック
    $('#<%=edittrade.ClientID%>').click(function (e) {
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
                  
            var trade_cd = cells.eq(1).text();//
            var trade_name = cells.eq(2).text();//
            var trade_kana = cells.eq(3).text();//
            var address_1 = cells.eq(4).text();//
            var address_2 = cells.eq(5).text();//
            var bank_cd = cells.eq(6).text();//
            var branch_cd = cells.eq(7).text();//
            var acc_cd = cells.eq(8).text();//
            var acc_no = cells.eq(9).text();//
            var acc_nm = cells.eq(10).text();//
            var mibarai_cd = cells.eq(11).text();//
            var shiharai_cd = cells.eq(12).text();//
            var keiyaku_no = cells.eq(13).text();//

            //空白変換
            trade_name = nbsp_func(trade_name);
            trade_kana = nbsp_func(trade_kana);
            acc_nm = nbsp_func(acc_nm);
            address_1 = nbsp_func(address_1);
            address_2 = nbsp_func(address_2);

            //データをクリア
          
            di.dialog('open');

            var wk_tradecd = document.getElementById("d_trade_cd");
            var wk_tradename = document.getElementById("d_trade_name");
            var wk_tardekana = document.getElementById("d_trade_kana");
            var wk_address1 = document.getElementById("d_address1");
            var wk_address2 = document.getElementById("d_address2");
            var wk_bankcd = document.getElementById("d_bank_cd");
            var wk_branchcd = document.getElementById("d_branch_cd");
            var wk_acccd = document.getElementById("d_acc_cd");
            var wk_accno = document.getElementById("d_acc_no");
            var wk_accnm = document.getElementById("d_acc_nm");
            var wk_mibaraicd = document.getElementById("d_mibarai_cd");
            var wk_shiharaicd = document.getElementById("d_shiharai_cd");
            var wk_keiyakuno = document.getElementById("d_keiyaku_no");
            var wk_KUBUN = document.getElementById("d_kubun");
            var wk_GYO = document.getElementById("d_gyo");


            wk_tradecd.value = trade_cd;
            wk_tradename.value = trade_name;
            wk_tardekana.value= trade_kana;
            wk_address1.value= address_1;
            wk_address2.value = address_2;
            wk_bankcd.value = bank_cd;
            wk_branchcd.value= branch_cd;
            wk_acccd.value= acc_cd;
            wk_accno.value= acc_no;
            wk_accnm.value= acc_nm;
            wk_mibaraicd.value= mibarai_cd;
            wk_shiharaicd.value= shiharai_cd;
            wk_keiyakuno.value= keiyaku_no;
                
            wk_KUBUN.innerHTML = "編集モード";
            wk_GYO.value = gyo;

            $('#d_trade_cd').prop('disabled', true);
            $('#d_trade_name').focus;


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

            var trade_cd = $(this).closest("tr").find("td").eq(1).text();
            var trade_name = $(this).closest("tr").find("td").eq(2).text();
            var trade_kana = $(this).closest("tr").find("td").eq(3).text();
            var address_1= $(this).closest("tr").find("td").eq(4).text();
            var address_2 = $(this).closest("tr").find("td").eq(5).text();
            var bank_cd = $(this).closest("tr").find("td").eq(6).text();
            var branch_cd = $(this).closest("tr").find("td").eq(7).text();

            var acc_cd = $(this).closest("tr").find("td").eq(8).text();
            var acc_no = $(this).closest("tr").find("td").eq(9).text();
            var acc_nm = $(this).closest("tr").find("td").eq(10).text();
            var mibarai_cd = $(this).closest("tr").find("td").eq(11).text();
            var shiharai_cd = $(this).closest("tr").find("td").eq(12).text();
            var keiyaku_no = $(this).closest("tr").find("td").eq(13).text();
          
            var wk_tradecd = document.getElementById("d_trade_cd");
            var wk_tradename = document.getElementById("d_trade_name");
            var wk_tardekana = document.getElementById("d_trade_kana");
            var wk_address1 = document.getElementById("d_address1");
            var wk_address2 = document.getElementById("d_address2");
            var wk_bankcd = document.getElementById("d_bank_cd");
            var wk_branchcd = document.getElementById("d_branch_cd");
            var wk_acccd = document.getElementById("d_acc_cd");
            var wk_accno = document.getElementById("d_acc_no");
            var wk_accnm = document.getElementById("d_acc_nm");
            var wk_mibaraicd = document.getElementById("d_mibarai_cd");
            var wk_shiharaicd = document.getElementById("d_shiharai_cd");
            var wk_keiyakuno = document.getElementById("d_keiyaku_no");
       
            var wk_KUBUN = document.getElementById("d_kubun");
            var wk_GYO = document.getElementById("d_gyo");

        
            wk_tradecd.value = trade_cd;
            wk_tradename.value = trade_name;
            wk_tardekana.value = trade_kana;
            wk_address1.value = address_1;
            wk_address2.value = address_2;
            wk_bankcd.value = bank_cd;
            wk_branchcd.value = branch_cd;
            wk_acccd.value = acc_cd;
            wk_accno.value = acc_no;
            wk_accnm.value = acc_nm;
            wk_mibaraicd.value = mibarai_cd;
            wk_shiharaicd.value = shiharai_cd;
            wk_keiyakuno.value = keiyaku_no;

         
            wk_KUBUN.innerHTML = "編集モード";
            wk_GYO.value = row;
            $('#d_trade_cd').prop('disabled', true);
            $('#d_trade_name').focus;
       
        });

        //内訳マスタ
        //行ダブルクリックで編集画面
        $("#MainContent_GridView1 td").dblclick(function () {
            //縦
            var row = $(this).closest('tr').index();
            //横
            var col = this.cellIndex;

            var trade_cd = $(this).closest("tr").find("td").eq(1).text();
            var trade_name = $(this).closest("tr").find("td").eq(2).text();
            var trade_kana = $(this).closest("tr").find("td").eq(3).text();
            var address_1 = $(this).closest("tr").find("td").eq(4).text();
            var address_2 = $(this).closest("tr").find("td").eq(5).text();
            var bank_cd = $(this).closest("tr").find("td").eq(6).text();
            var branch_cd = $(this).closest("tr").find("td").eq(7).text();

            var acc_cd = $(this).closest("tr").find("td").eq(8).text();
            var acc_no = $(this).closest("tr").find("td").eq(9).text();
            var acc_nm = $(this).closest("tr").find("td").eq(10).text();
            var mibarai_cd = $(this).closest("tr").find("td").eq(11).text();
            var shiharai_cd = $(this).closest("tr").find("td").eq(12).text();
            var keiyaku_no = $(this).closest("tr").find("td").eq(13).text();

            //空白変換
            tarde_name = nbsp_func(trade_name);
            tarde_kana = nbsp_func(trade_kana);
            address_1 = nbsp_func(address_1);
            address_2 = nbsp_func(address_2);
            acc_nm = nbsp_func(acc_nm);

            //編集画面オープン
            di.dialog('open');
      
            var wk_tradecd = document.getElementById("d_trade_cd");
            var wk_tradename = document.getElementById("d_trade_name");
            var wk_tardekana = document.getElementById("d_trade_kana");
            var wk_address1 = document.getElementById("d_address1");
            var wk_address2 = document.getElementById("d_address2");
            var wk_bankcd = document.getElementById("d_bank_cd");
            var wk_branchcd = document.getElementById("d_branch_cd");
            var wk_acccd = document.getElementById("d_acc_cd");
            var wk_accno = document.getElementById("d_acc_no");
            var wk_accnm = document.getElementById("d_acc_nm");
            var wk_mibaraicd = document.getElementById("d_mibarai_cd");
            var wk_shiharaicd = document.getElementById("d_shiharai_cd");
            var wk_keiyakuno = document.getElementById("d_keiyaku_no");

            var wk_KUBUN = document.getElementById("d_kubun");
            var wk_GYO = document.getElementById("d_gyo");

            wk_tradecd.value = trade_cd;
            wk_tradename.value = trade_name;
            wk_tardekana.value = trade_kana;
            wk_address1.value = address_1;
            wk_address2.value = address_2;
            wk_bankcd.value = bank_cd;
            wk_branchcd.value = branch_cd;
            wk_acccd.value = acc_cd;
            wk_accno.value = acc_no;
            wk_accnm.value = acc_nm;
            wk_mibaraicd.value = mibarai_cd;
            wk_shiharaicd.value = shiharai_cd;
            wk_keiyakuno.value = keiyaku_no;

            wk_KUBUN.innerHTML = "編集モード";
            wk_GYO.value = row;

            $('#d_trade_cd').prop('disabled', true);
            $('#d_trade_name').focus;
            
        });
   
    
    // 行削除ボタンのイベントハンドラ
    $('#<%=deltrade.ClientID%>').click(function () {
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

});

</script>
    <div style="width:2000px;margin-left :-400px">
      <div class="index1">
    取引先登録
      </div>
<asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Width="2000px"
        Height="800px" >
    <asp:GridView ID="GridView1" runat="server"  Width="2000px">
         <Columns>
            <asp:TemplateField HeaderText="選択" HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate >
                    <asp:RadioButton ID="RadioButton1" runat="server"  />
                    </ItemTemplate>
         </asp:TemplateField>
          </Columns>
        <HeaderStyle cssclass="" Wrap="False" BackColor="#99CCFF" />
    </asp:GridView>
    </asp:Panel>
  
<div style="margin-top:5px">
<asp:button runat="server"  class="btn btn-default" id="newtrade" text="新規" style="height:50px"/>
<asp:button runat="server"  class="btn btn-default" id="edittrade"  text="編集" style="height:50px"/>
<asp:button runat="server"  class="btn btn-default" id="deltrade" text="削除" style="height:50px"/>
<asp:button runat="server"  class="btn btn-default" id="closetrade" text="閉じる"  style="height:50px"/>
</div>
    
<div id="dialog_input" title="新規/編集">
<p>
<label id="d_kubun" style="color:red;">種別</label>
</p>
<div class="hi">
<input type="hidden" id="d_gyo" />
<table border ="0">
<tr>
<td>取引先ｺｰﾄﾞ</td><td><input type="text" maxlength="6" id="d_trade_cd" class="input-sm" style="font-size:small;width:100px;"/></td>
</tr>
<tr>
<td>取引先名</td><td><input type="text" maxlength="50" id="d_trade_name" style="font-size:smaller;" />
</tr>
<tr>
<td>カナ名称</td><td><input type="text" maxlength="80" id="d_trade_kana" class="input-sm" style="font-size:small;width:100px;"/></td>
</tr>
<tr>
<td>住所１</td><td><input type="text" maxlength="50" id="d_address1" class="input-sm" style="font-size:small;width:300px;" /></td>
</tr>
<tr>
<td>住所２</td><td><input type="text" maxlength="50" id="d_address2" style="font-size:smaller;color:blue" /></td>
</tr>
<tr>
<td>銀行ｺｰﾄﾞ</td><td><input type="text" maxlength="4" id="d_bank_cd" style="font-size:smaller;color:blue" /></td>
</tr>
<tr>
<td>支店ｺｰﾄﾞ</td><td><input type="text" maxlength="3" id="d_branch_cd" style="font-size:smaller;color:blue" /></td>
</tr>
<tr>
<td>登録ｺｰﾄﾞ</td><td><input type="text" maxlength="1" id="d_acc_cd" style="font-size:smaller;color:blue" onkeypress="return CheckNumNumelic();" /></td>
</tr>
<tr>
<td>登録No</td><td><input type="text" maxlength="7" id="d_acc_no" style="font-size:smaller;color:blue" /></td>
</tr>
<tr>
<td>登録名</td><td><input type="text" maxlength="40" id="d_acc_nm" style="font-size:smaller;color:blue" /></td>
</tr>
<tr>
<td>未払ｺｰﾄﾞ</td><td><input type="text" maxlength="5" id="d_mibarai_cd" style="font-size:smaller;color:blue" /></td>
</tr>
<tr>
<td>支払先ｺｰﾄﾞ</td><td><input type="text" maxlength="10" id="d_shiharai_cd" style="font-size:smaller;color:blue" /></td>
</tr>
<tr>
<td>契約No</td><td><input type="text" maxlength="8" id="d_keiyaku_no" style="font-size:smaller;color:blue" /></td>
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
<div id="dialog_delete" title="メッセージ">
削除します　よろしいですか?
</div>
</div>    

</asp:Content>
