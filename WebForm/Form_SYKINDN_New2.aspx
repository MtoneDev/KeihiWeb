﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Form_SYKINDN_New2.aspx.vb" Inherits="KeihiWeb.Form_SYKINDN_New2" %>
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
          top:0px;
          width:100px;
          background-color:yellow;
          left:70px;
          font-size :medium ; 
   }
   .zandaka{
       position:absolute;
       top:0px;
       left:350px;
       background-color:orange;
       text-align:right;
       max-width :150px;
       font-size :medium ; 
   }
   .denban_lbl{
          position:absolute;
          top:0px;
          left:0px;
          }
   .zandaka_lbl{
       position:absolute;
       top:0px;
       left:300px;
   }
   .header_group1{  
    top:80px; 
    }
   .panel{
       position:absolute;
       height:80px;
       top:50px;
       width:100%;
       padding-top:5px;
   }
   .meisai{
          position:relative;
          top:120px;
   }
   
   .goukei_ran {
       position:absolute;
       left:0px;
       margin-top:10px;
   }
   #d_user_cd{
          position:relative;
          left:14px;
          width:100px; 
          margin-left:5px;
   }
   #user_serach{
          position:relative;
          left:15px;
     
   }   
  .gyobtn{
          position:absolute;
          top:325px;
   }   
  

</style>
<script type="text/javascript" src="../Scripts/WebForms/STSCommon/common.js" ></script>
<script type="text/javascript" src="../Scripts/WebForms/STSCommon/keihi_user.js" ></script>
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

$(function() {

    var errchk_date;
    var errchk_id;

  
    //更新項目作成関数（グリッドの値をhiddn項目へセットする関数）
    function value_set() {
        $('#<%=keihi_goukei.ClientID%>').val($('#<%=keihi_goukeiran.ClientID%>').val());
        $('#<%=zei_goukei.ClientID%>').val($('#<%=zei_goukeiran.ClientID%>').val());
        $('#<%=kin_goukei.ClientID%>').val($('#<%=kin_goukeiran.ClientID%>').val());
        
        var div_element = document.getElementById('hidden5');
                   //動的に明細項目(hidden)サーバー転送用項目生成する
        var tr = $('#<%=GridView1.ClientID%> tr');
        for (var i = 1, l = tr.length ; i < l; i++) {
            var cells = tr.eq(i).children();//1行目から順にth、td問わず列を取得
            var bumoncd = cells.eq(1).text();//
            var bumonnm = cells.eq(2).text();//
            var shiharai = cells.eq(3).text();//
            var kamokucd = cells.eq(4).text();
            var kamokunm = cells.eq(5).text();
            var uchiwakecd = cells.eq(6).text();
            var uchiwakenm = cells.eq(7).text();
            var tekiyo = cells.eq(8).text();
            var zkubun = cells.eq(9).text();
            var zname = cells.eq(10).text();
            var keihi = cells.eq(11).text();
            var zei = cells.eq(12).text();
            var kin = cells.eq(13).text();

            if (bumoncd === ' ') {
            } else {

                //部門
                var namewk = 'h_bumon_cd' + i;
                var wkelment_div = document.createElement("div_bumon_h" + i);
                wkelment_div.innerHTML = '<input type="hidden" name="' + namewk + '"' + 'id="' + namewk + '">';
                var ojb = document.getElementById("hidden5");
                ojb.appendChild(wkelment_div);
                document.getElementById(namewk).value = bumoncd;

                //部門
                var namewk = 'h_bumon_nm' + i;
                var wkelment_div = document.createElement("div_bumonnm_h" + i);
                wkelment_div.innerHTML = '<input type="hidden" name="' + namewk + '"' + 'id="' + namewk + '">';
                var ojb = document.getElementById("hidden5");
                ojb.appendChild(wkelment_div);
                document.getElementById(namewk).value = bumonnm;


                //支払
                var namewk = 'h_shiharai' + i;
                var wkelment_div = document.createElement("div_shiharai_h" + i);
                wkelment_div.innerHTML = '<input type="hidden" name="' + namewk + '"' + 'id="' + namewk + '">';
                var ojb = document.getElementById("hidden5");
                ojb.appendChild(wkelment_div);
                document.getElementById(namewk).value = shiharai;

                //科目
                var namewk = 'h_kamoku_cd' + i;
                var wkelment_div = document.createElement("div_kmokucd_h" + i);
                wkelment_div.innerHTML = '<input type="hidden" name="' + namewk + '"' + 'id="' + namewk + '">';
                var ojb = document.getElementById("hidden5");
                ojb.appendChild(wkelment_div);
                document.getElementById(namewk).value = kamokucd;

                //科目
                var namewk = 'h_kamoku_nm' + i;
                var wkelment_div = document.createElement("div_kmokunm_h" + i);
                wkelment_div.innerHTML = '<input type="hidden" name="' + namewk + '"' + 'id="' + namewk + '">';
                var ojb = document.getElementById("hidden5");
                ojb.appendChild(wkelment_div);
                document.getElementById(namewk).value = kamokunm;

                //内訳
                var namewk = 'h_uchiwake_cd' + i;
                var wkelment_div = document.createElement("div_uchiwakecd_h" + i);
                wkelment_div.innerHTML = '<input type="hidden" name="' + namewk + '"' + 'id="' + namewk + '">';
                var ojb = document.getElementById("hidden5");
                ojb.appendChild(wkelment_div);
                document.getElementById(namewk).value = uchiwakecd;

                //内訳
                var namewk = 'h_uchiwake_nm' + i;
                var wkelment_div = document.createElement("div_uchiwakenm_h" + i);
                wkelment_div.innerHTML = '<input type="hidden" name="' + namewk + '"' + 'id="' + namewk + '">';
                var ojb = document.getElementById("hidden5");
                ojb.appendChild(wkelment_div);
                document.getElementById(namewk).value = uchiwakenm;



                ////摘要
                var namewk = 'h_tekiyo' + i;
                var wkelment_div = document.createElement("div_tekiyo_h" + i);
                wkelment_div.innerHTML = '<input type="hidden" name="' + namewk + '"' + 'id="' + namewk + '">';
                var ojb = document.getElementById("hidden5");
                ojb.appendChild(wkelment_div);
                document.getElementById(namewk).value = tekiyo;


                ////税区分
                var namewk = 'h_zkubun' + i;
                var wkelment_div = document.createElement("div_zekubun_h" + i);
                wkelment_div.innerHTML = '<input type="hidden" name="' + namewk + '"' + 'id="' + namewk + '">';
                var ojb = document.getElementById("hidden5");
                ojb.appendChild(wkelment_div);
                document.getElementById(namewk).value = zkubun;

                ////税区分名
                var namewk = 'h_zkubunnm' + i;
                var wkelment_div = document.createElement("div_zekubunnm_h" + i);
                wkelment_div.innerHTML = '<input type="hidden" name="' + namewk + '"' + 'id="' + namewk + '">';
                var ojb = document.getElementById("hidden5");
                ojb.appendChild(wkelment_div);
                document.getElementById(namewk).value = zname;


                ////経費
                var namewk = 'h_keihi' + i;
                var wkelment_div = document.createElement("div_keihi_h" + i);
                wkelment_div.innerHTML = '<input type="hidden" name="' + namewk + '"' + 'id="' + namewk + '">';
                var ojb = document.getElementById("hidden5");
                ojb.appendChild(wkelment_div);
                document.getElementById(namewk).value = keihi;


                ////消費税
                var namewk = 'h_zei' + i;
                var wkelment_div = document.createElement("div_zei_h" + i);
                wkelment_div.innerHTML = '<input type="hidden" name="' + namewk + '"' + 'id="' + namewk + '">';
                var ojb = document.getElementById("hidden5");
                ojb.appendChild(wkelment_div);
                document.getElementById(namewk).value = zei;


                ////金額
                var namewk = 'h_kin' + i;
                var wkelment_div = document.createElement("div_kin_h" + i);
                wkelment_div.innerHTML = '<input type="hidden" name="' + namewk + '"' + 'id="' + namewk + '">';
                var ojb = document.getElementById("hidden5");
                ojb.appendChild(wkelment_div);
                document.getElementById(namewk).value = kin;

            }

        }

     };
  
    ///**********　　　
    //初期セット
    ///**********　　　
    //明細入力時のボタンタイトル
    var titlename = '追加';

    //datepiclker
    $.datepicker.setDefaults($.datepicker.regional['ja']);

    $('#d_shiharai').css('ime-mode', 'active');
    $('#d_tekiyo').css('ime-mode', 'active');
    $('#d_kan_code').css('ime-mode', 'inactive');
    $('#d_uti_code').css('ime-mode', 'inactive');
    $('#d_kingaku').css('ime-mode', 'inactive');



    $('#d_bumon_id').click(function () {
        $(this).select();
    });

    $('#d_kan_code').click(function () {
        $(this).select();
    });
    $('#d_uti_code').click(function () {
        $(this).select();
    });
    $('#d_kingaku').click(function () {
        $(this).select();
    });
    $('#d_shiharai').click(function () {
        $(this).select();
    });
    $('#d_tekiyo').click(function () {
        $(this).select();
    });

    $('#<%=kounyu_date.ClientID%>').click(function () {
        $(this).select();
    });

    $('#d_user_cd').click(function () {
        $(this).select();
    });

    $('#<%=kounyu_date.ClientID%>').datepicker({
      dateFormat: 'yy/mm/dd'
    });
   
    //カンマ編集
    
    kanma_func($('#<%=zandaka.ClientID%>'))
    
    //タイトルの変更
     if ($('#<%=syubetu.ClientID%>').val() === '新規') {
            $('#title').text(' 出金伝票入力（新規）');
            $('#<%=kounyu_date.ClientID%>').datepicker('setDate', new Date());
            $('#<%=button_delete.ClientID%>').prop('disabled', true);
   
     } else {
            $('#title').text(' 出金伝票入力（訂正）');
            syukei_calc(11, 12, 13);
            $('#<%=button_delete.ClientID%>').prop('disabled', false);
    }

        
    var wk_GYO = document.getElementById("d_gyo");
    wk_GYO.value = '';

    $('#d_user_cd').val($('#<%=auth_user_id.ClientID%>').val());

    $('#d_user_name').val($('#<%=auth_user_name.ClientID%>').val());

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
    var di_YesNo = $('#dialog_YesNo');
    di_YesNo.dialog({

        modal: false,
        width: 250,
        height: 170,
        autoOpen: false,
        buttons: {

            'はい': function () {
                $(this).dialog('close');
            
            },

            'いいえ': function () {
                $(this).dialog('close');

                $('#err_chk_flg').val('1');
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

    //伝票削除確認ダイアログ
    var di_denpyo_delete = $('#dialog_denpyo_delete');
    //ダイアログを初期化（自動オープンしない）
    di_denpyo_delete.dialog({
        modal: false,
        width: 350,
        height: 200,
        autoOpen: false,
        buttons: {
            '削除': function () {
                var jibumoncd = document.getElementById("<%=jibumoncd.ClientID%>").value;
                var slipno = document.getElementById("<%=denban1.ClientID%>").value;
                
                var manageno = document.getElementById("<%=manageno.ClientID%>").value;
                var JSONdata = {
                    jibumoncd:jibumoncd,
                    manageno:manageno
                };

                url_wk = 'Form_SYKINDN_New.aspx/DeleteDenpyoJOSNData';
                $.ajax({
                    type: "POST",
                    url: url_wk,
                    data: JSON.stringify(JSONdata),
                    datatype: 'json',
                    timeout: 5000,
                    contentType: 'application/json; charset=utf-8',
                    success: function (msg) {
                        di_denpyo_delete.dialog('close');
                        var member = msg.d;
                        var result = member.result;
                        //$('#<%=msgbox.ClientID%>').text('伝票[' + slipno + ']を削除しました')         
                        // var msglabel = document.getElementById("msglabel");
                        //var wk_msg = document.getElementById("msg");
                        //グリッドに更新値をセット
                        //結果をメッセージボッツクスへ
                        return msg;
                    },
                    error: function (data, errorThrown) {
                        $('#msg').text('異常終了' + errorThrown);
                        di_msg.dialog('open');
                        console.log(errorThrown);
                        return false;
                    }
                }).done(function (data) {

                      $('#<%=move_itiran.ClientID%>').trigger('click');
             
                });

            

            },
            '閉じる': function () {
                //window.location.reload();
                $(this).dialog('close');

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
       
    });

    $('#utiwake').click(function () {
        di_utiwake.dialog('close');
        $('#d_tekiyo').focus();
    });


    //**********
    //*****明細登録画面******
    //**********

    var di = $('#dialog_syukin');

    //ダイアログを初期化（自動オープンしない）
    di.dialog({
            
        modal: true,
        width: 650,
        height: 560,
        autoOpen: false,
        buttons: [
            
            //ダイアログの追加、訂正ボタン
            //'更新': function () {
            //
            {
       
                text: titlename,
                  click: function () {
                      var msg_red = $('#err_msg_lbl').val();

                      var bumon_cd = document.getElementById("d_bumon_id").value;
                      var bumon_nm = document.getElementById("d_bumon_name").innerHTML;
                      var shiharai = document.getElementById("d_shiharai").value;
                      var kamoku_cd = document.getElementById("d_kan_code").value;
                      var kamoku_nm = document.getElementById("d_kan_name").innerHTML;
                      var uchi_cd = document.getElementById("d_uti_code").value;
                      var uchi_nm = document.getElementById("d_uti_name").innerHTML;
                      var tekiyo = document.getElementById("d_tekiyo").value;
                      var gaku = document.getElementById("d_kingaku").value;
                      var keihi = document.getElementById("d_keihi").value;
                      var zei = document.getElementById("d_syozei").value;
                      var kin = document.getElementById("d_kingaku").value;

                      var err_flg = '';
                      //***********
                      //入力チェック
                      //***********
         
                      if (shiharai.length > 50) {
                          $('#msg').text('支払先　文字数オーバーです');
                          err_flg = '1';
                      }

                      if (tekiyo.length > 0 ) {
                          if (tekiyo.length > 40) {
                              $('#msg').text('摘要欄　文字数オーバーです');
                              err_flg = '1';
                          }
                      }

                  
                      if (shiharai.length === 0) {
                          $('#msg').text('必要項目が未入力です');
                          err_flg = '1';
                      }

                      if (kamoku_cd.length === 0) {
                          err_flg = '1';
                          $('#msg').text('必要項目が未入力です');

                      }
                      
                      if (uchi_cd.length === 0) {
                          if ($('#d_uti_code').prop('disabled') === false) {
                              $('#msg').text('必要項目が未入力です');
                              err_flg = '1';

                          }

                      }

                      // 金額チェック
                      var gaku = gaku.replace(/,/g, "");

                      if (gaku.length > 10) {
                          $('#msg').text('金額欄　文字数オーバーです');
                          err_flg = '1';
                      }

                      var pattern = /^[-]?([1-9]\d*|0)(\.\d+)?$/;
                      if (pattern.test(gaku)) {
                      } else {
                          $('#msg').text('金額ﾞが不適切です');
                          di_msg.dialog('open');
                          return false;
                      }


                      if (err_flg === '1') {
                          di_msg.dialog('open');
                          return false;
                      }

                      ///マスタ存在チェック
                      if (msg_red.length > 0) {
                          $('#msg').text('ｺｰﾄﾞが不適切です');
                          di_msg.dialog('open');
                          return false;
                      }


                      
                      kanma_func($('#d_kingaku'));

                      var zkubun = '4';
                      var zname = 'その他';

                      if ($('input[name=z_kubun]:checked').val() === '0') {
                          zkubun = '1';
                          zname = '内税';
                      }
                      if ($('input[name=z_kubun]:checked').val() === '1') {
                          zkubun = '3';
                          zname = '非課税';
                      }

                      var wk;
                      var rec = 0;
                      var gyo = 0;

                      var select_GYO = document.getElementById("d_gyo");

                      //sinnki
                      //gridviewの登録レコード数を取得

                    //  value_set2();


                      //select_GYOが空で新規登録
                      if (select_GYO.value === '') {
                          var tr = $('#<%=GridView1.ClientID%> tr');

                          for (var i = 1, l = tr.length  ; i < l; i++) {
                              var cells = tr.eq(i).children();//1行目から順にth、td問わず列を取得
                              var wk = cells.eq(1).text();//
                              if (wk === ' ') {
                              } else {
                                  rec = parseInt(rec) + 1;
                              }


                          }

                　　　　　//次の行に追加
                          rec = parseInt(rec) + 1;
                          //最大行数チェック
                          var max = $('#<%=h_maxgyo.ClientID%>').val();
                         
                          if (rec - 1 <= max) {
                          } else {
                              $('#msg').text('最大行数を超えました');
                              di_msg.dialog('open');
                              return false
                          }
                     

                      } else {
                          //select_GYOが空以外で修正登録
                          rec = parseInt(select_GYO.value);

                      }

                      
                      $('#wk_bumoncd').val(bumon_cd);
                      $('#wk_bumonnm').val(bumon_nm);
                      $('#wk_shiharai').val(shiharai);
                      $('#wk_kamokucd').val(kamoku_cd);
                      $('#wk_kamokunm').val(kamoku_nm);
                      $('#wk_uchiwakecd').val('');
                      $('#wk_uchiwakenm').val('');
                      $('#wk_tekiyo').val('');
                      $('#wk_zkubun').val('');
                      $('#wk_zkubunnm').val('');

                 
                      var tr = $("#MainContent_GridView1 tr");
                      var cells = tr.eq(rec).children();
                      cells.eq(1).text(bumon_cd);
                      cells.eq(2).text(bumon_nm);
                      cells.eq(3).text(shiharai);
                      cells.eq(4).text(kamoku_cd);
                      cells.eq(5).text(kamoku_nm);
                      cells.eq(6).text(uchi_cd);
                      cells.eq(7).text(uchi_nm);
                      cells.eq(8).text(tekiyo);
                      cells.eq(9).text(zkubun);
                      cells.eq(10).text(zname);
                      cells.eq(11).text(keihi);
                      cells.eq(12).text(zei);
                      cells.eq(13).text(kin);


                      //合計欄再計算
                      syukei_calc(11, 12, 13);

                 
                      if ($('#<%=syubetu.ClientID%>').val() === '新規') {
                          $('#d_tekiyo').val('');
                          $('#d_kingaku').val('');
                          $('#d_kin').val('');
                          $('#d_gokei').val('');
                          $('#d_keihi').val('');
                          $('#d_syozei').val('');

                      } else {
                          $(this).dialog('close');
                      }
                  }
              }
              ,
              {
                  text:'閉じる',
                  click:function () {
                      $(this).dialog('close');
                  }
              }
            ]
    });
    //******
    //****行追加ボタン
    //******
    $('#<%=gyo_new.ClientID%>').click(function () {
            //明細登録画面

            $('#<%=button_kousin.ClientID%>').prop('disabled', 'true');

            if (errchk_date==='1' || errchk_id === '1') {

                $('#msg').text('エラーが存在します');
                di_msg.dialog('open');

                 return false;

            }

            var wk_GYO = document.getElementById("d_gyo");
            wk_GYO.value = '';
            $('#<%=msgbox.ClientID%>').text('');
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
            $('#d_keihi').val('0')
            $('#d_syozei').val('0');
            $('#d_gokei').val('0');
    
            di.dialog('open');
            
            
            return false;
        });
    //////エンターによる命令防止

    $('#Container').keypress(function (ev) {
        if ((ev.which && ev.which === 13) || (ev.keyCode && ev.keyCode === 13)) {
            return false
          
        } else {
            return true

        }
    });
    
    ///
    //行削除ダイアログ
        var di_delete = $('#dialog_delete');
    //ダイアログを初期化（自動オープンしない）
        di_delete.dialog({
            modal: false,
            width: 250,
            height: 200,
            autoOpen: false,
            buttons: {
                '削除': function () {
                    //選択行を取得

                    var wk_GYO = document.getElementById("d_gyo");
                    var GYO = wk_GYO.value;
                    var gyo = parseInt(GYO);
                    var tr = $('#<%=GridView1.ClientID%> tr');
                    var max = $('#<%=h_maxgyo.ClientID%>').val();
                    var max_int = parseInt(max)+1;
                        //tr.eq(gyo).remove();//行削除
                        //削除行から下の行を上にコピー                   
                        for (var i = gyo, l = tr.length - 1; i < l; i++) {

                            var cells = tr.eq(i).children();
                            var sita_cells = tr.eq(i + 1).children();
                            cells.eq(1).text(sita_cells.eq(1).text());//
                            cells.eq(2).text(sita_cells.eq(2).text()); //
                            cells.eq(3).text(sita_cells.eq(3).text());//
                            cells.eq(4).text(sita_cells.eq(4).text());//
                            cells.eq(5).text(sita_cells.eq(5).text());//
                            cells.eq(6).text(sita_cells.eq(6).text());//
                            cells.eq(7).text(sita_cells.eq(7).text());//
                            cells.eq(8).text(sita_cells.eq(8).text());//
                            cells.eq(9).text(sita_cells.eq(9).text());//
                            cells.eq(10).text(sita_cells.eq(10).text());//
                            cells.eq(11).text(sita_cells.eq(11).text());//
                            cells.eq(12).text(sita_cells.eq(12).text());//
                            cells.eq(13).text(sita_cells.eq(13).text());//

                        }
                        //最終行に空をセット
                        var cells = tr.eq(max_int).children();
                        cells.eq(1).text(' ');//
                        cells.eq(2).text(''); //
                        cells.eq(3).text('');//
                        cells.eq(4).text('');//
                        cells.eq(5).text('');//
                        cells.eq(6).text('');//
                        cells.eq(7).text('');//
                        cells.eq(8).text('');//
                        cells.eq(9).text('');//
                        cells.eq(10).text('');//
                        cells.eq(11).text('');//
                        cells.eq(12).text('');//
                        cells.eq(13).text('');//

                        //集計欄再計算
                        syukei_calc(11, 12, 13);
                        //選択ボタンクリア
                        $('#<%=GridView1.ClientID%> input:radio').each(function () {
                            $(this).prop('checked', false);
                        });
                  
                    $(this).dialog('close');
                    
                },
                '閉じる': function () {
                    //window.location.reload();
                    $(this).dialog('close');

                }
            }
        });

    //購入日を変更したら更新ボタンをＦＡＬＳＥ
    $('#<%=kounyu_date.ClientID%>').on({
        change: function () {
            $('#err_msg_lbl').val('');
            errchk_date = '';

           if ($('#<%=kounyu_date.ClientID%>').val().length > 10) {
                $('#<%=gyo_new.ClientID%>').prop('disabled', false);
                $('#msg').text('購入日の値が不正です(yyyy/MM/dd)');
                $('#err_msg_lbl').val('購入日エラー');
                errchk_date = '1';
                di_msg.dialog('open');
                return false;
            }


            $('#<%=button_kousin.ClientID%>').prop('disabled', true);
            $('#<%=gyo_new.ClientID%>').prop('disabled', false);
            var datestr = $('#<%=kounyu_date.ClientID%>').val();

            if (datestr.match(/[0-9]{8}/)) {
                str1 = datestr.substring(0, 4) + "/" + datestr.substring(4, 6) + "/" + datestr.substring(6, 8)
                $('#<%=kounyu_date.ClientID%>').val(str1);
                datestr = $('#<%=kounyu_date.ClientID%>').val();
            } else {
               // alert($('#<%=kounyu_date.ClientID%>').val());
            }
            

            var vYear = datestr.substr(0, 4) - 0;
            // Javascriptは、0-11で表現
            var vMonth = datestr.substr(5, 2) - 1;
            var vDay = datestr.substr(8, 2) - 0;
            // 月,日の妥当性チェック
            if (vMonth >= 0 && vMonth <= 11 && vDay >= 1 && vDay <= 31) {
                var vDt = new Date(vYear, vMonth, vDay);
                if (isNaN(vDt)) {
                    $('#<%=gyo_new.ClientID%>').prop('disabled', false);
                    errchk_date = '1';
                    $('#msg').text('購入日の値が不正です(yyyy/MM/dd)');
                    $('#err_msg_lbl').val('購入日エラー');
                    di_msg.dialog('open');
                    return false;
                } else if (vDt.getFullYear() == vYear
                 && vDt.getMonth() == vMonth
                 && vDt.getDate() == vDay) {
                    return true;
                } else {
                    $('#<%=gyo_new.ClientID%>').prop('disabled', false);
                    errchk_date = '1';
                    $('#msg').text('購入日の値が不正です(yyyy/MM/dd)');
                    $('#err_msg_lbl').val('購入日エラー');
                    di_msg.dialog('open');
                    return false;
                }
            } else {
                $('#<%=gyo_new.ClientID%>').prop('disabled', false);
                errchk_date = '1';
                $('#err_msg_lbl').val('購入日エラー');
                $('#msg').text('購入日の値が不正です(yyyy/MM/dd)');
                di_msg.dialog('open');

                return false;
            }

             if ($('#<%=kounyu_date.ClientID%>').val().length < 10) {
                $('#<%=gyo_new.ClientID%>').prop('disabled', false);
                 errchk_date = '1';
                 $('#msg').text('購入日の値が不正です(yyyy/MM/dd)');
                $('#err_msg_lbl').val('購入日エラー');
                di_msg.dialog('open');
                return false;
            }

        }
    });

 
    //*****
    //****確認ボタン
    //*****

    $('#button_kakunin').click(function () {

        var dom_obj = document.getElementById("hidden5");
        dom_obj.textContent = null;

        if (errchk_date === '1' || errchk_id === '1') {

            $('#msg').text('エラーが存在します');
            di_msg.dialog('open');

            return false;

        }

        var tr_chk = $('#<%=GridView1.ClientID%> tr');
        var cells_chk = tr_chk.eq(1).children();//1行目から順にth、td問わず列を取得
        var bumoncd_chk = cells_chk.eq(2).text();//
       
        if (bumoncd_chk.length >1 ) {
            //更新ボタンを有効
            $('#<%=button_kousin.ClientID%>').prop('disabled', false);
        } else {

            $('#msg').text('データが登録されていません');
            di_msg.dialog('open');

            return false;
        }


        //***
        //伝票番号の取得 
        //キー生成（自部門コードと購入日YYMM)
        //***


        var wkden_no = $('#<%=denban1.ClientID%>').val();

        if ($('#<%=syubetu.ClientID%>').val() === '新規') {

            var den_no = $('#<%=denban1.ClientID%>');

            var jibumoncd = document.getElementById("auth_bumon_cd").value;
            var koubay_date = $('#<%=kounyu_date.ClientID%>').val();
            var koubay_date_val = koubay_date.replace(/\u002f/g, '');

            var YM = koubay_date_val.substring(2, 6);
            var inputym = YM;

            var url_wk = 'Form_SYKINDN_New.aspx/GetJOSNDenpyo_NO';
            var JSONdata = {
                //code: bumon_cd
                jibumoncd: jibumoncd,
                ym: inputym
            };
            //伝票番号の取得 
            $.ajax({
                type: "POST",
                url: url_wk,
                data: JSON.stringify(JSONdata),
                datatype: 'json',
                async: false,
                timeout: 5000,
                contentType: 'application/json; charset=utf-8',
                success: function (result) {
                    var member = result.d;
                    console.log(member);
                    var no = parseInt(member[0].MAXSLIP_NO) + 1;
                    //wk_BUMON_NAME.innerHTML = member[0].BUMON_NM;

                    den_no.val(no);
                    $('#<%=h_denban.ClientID%>').val(no);

                    //***********
                    //更新項目セット関数の呼び出し
                    //***********

                    value_set();

                    return result;

                },
                error: function (data, errorThrown) {
                    $('#msg').text('異常終了！！' + errorThrown);
                    di_msg.dialog('open');
                    console.log(errorThrown);
                    return false;

                }
            }).done(function (msg) {

            });
        }
        else {
            //訂正のとき
            //***********
            //更新項目セット関数の呼び出し
            //***********
            $('#<%=h_denban.ClientID%>').val(wkden_no);

            value_set();

        }
        
        $('#<%=button_kousin.ClientID%>').trigger('click');

        
        return false;
     });



    //*****
    //***伝票ごと削除ボタン
    //*****

    $('#<%=button_delete.ClientID%>').click(function () {

        if (errchk_date === '1' || errchk_id === '1') {

            $('#msg').text('エラーが存在します');
            di_msg.dialog('open');

            return false;

        }

        var slipno = document.getElementById("<%=denban1.ClientID%>").value;
        $('#dialog_denpyo_delete').text('伝票[' + slipno + ']を削除します　よろしいですか?');

        di_denpyo_delete.dialog('open');
        return false;
     });


    //ユーザーチェンジイベント
    //ユーザー名表示
    $('#d_user_cd').on({
        change: function () {
            $('#err_msg_lbl').val('');
            errchk_id = '';

            $('#<%=gyo_new.ClientID%>').prop('disabled', false);

            $('#<%=button_kousin.ClientID%>').prop('disabled', true);
        
            var user_cd = document.getElementById("d_user_cd").value;
            var user_name = document.getElementById("d_user_name").value;

            var wk_USER_NAME = document.getElementById("d_user_name");
            if (user_cd.length > 0) {
                //ユーザー表示用関数
                $('#d_user_name').func_user_nm(user_cd, wk_USER_NAME);
            } else {
                $('#d_user_name').val('');
                errchk_id = '1';
            }
            if (user_cd.length > 6 || user_cd.length === 0) {

                $('#msg').text('ユーザーIDの値が不正です');
                errchk_id = '1';
                di_msg.dialog('open');
                $('#<%=gyo_new.ClientID%>').prop('disabled', true);

            }
            if ($('#err_msg_lbl').val().length > 0) {

                errchk_id = '1';
              
            }



       }
    });


    //ユーザー選択画面オープン
    $('#user_serach').click(function (e) {

        $('#err_msg_lbl').val('');
        //データをクリア
        $('table#user *').remove();
        errchk_id = '';

        $.ajax({
            type: "POST",
            url: "Form_SYKINDN_New.aspx/GetJOSNData_User",
            contentType: "application/json;charset=utf-8",
            data: {},
            dataType: "json",
            success: function (data) {

                if (data.d.length > 0) {
                    $('#user').append("<tr style='background-color:#6699FF'><th>社員ｺｰﾄﾞ</th><th>氏名</th><th>部門ｺｰﾄﾞ</th></tr>");

                    for (var i = 0; i < data.d.length; i++) {

                        $('#user').append("<tr onClick='mClickTR_user(this)'><td>" + data.d[i].USER_ID + "</td> <td>" + data.d[i].USER_NAME + "</td> <td>" + data.d[i].BUMON_CD + "</td></tr>");

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

        di_user.dialog('open');

    });
    
    //部門チェンジイベント
    //部門名表示
    $('#d_bumon_id').on({
        change: function () {
            $('#err_msg_lbl').val('');
            
            $('#d_bumon_name').css('color', 'black');

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

        $('#err_msg_lbl').val('');
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
                    $('#bumon').append("<tr style='background-color:#6699FF'><th>部門ｺｰﾄﾞ</th><th>部門名</th><th>債務部門</th></tr>");

                    for (var i = 0; i < data.d.length; i++) {
                        $('#bumon').append("<tr onClick='mClickTR_bumon(this)'><td>" + data.d[i].BUMON_CD + "</td> <td>" + data.d[i].BUMON_NM + "</td> <td>" + data.d[i].SAIMU_BMN + "</td></tr>");

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

        di_bumon.dialog('open');

    });

    //税区分ラジオボタン変更時イベントリスナー
    $('input[name="z_kubun"]:radio').change(function () {
            if ($('input[name="z_kubun"]:eq(0)').is(':checked')) {
                $('#d_keihi').css('background-color', 'orange');
                $('#d_syozei').css('background-color', 'orange');
                calc_kingaku($('#d_kingaku'), $('#<%=kounyu_date.ClientID%>').val());
            } else {
                $('#d_keihi').css('background-color', 'lightgray');
                $('#d_syozei').css('background-color', 'lightgray');
                calc_kingaku($('#d_kingaku'), $('#<%=kounyu_date.ClientID%>').val());
            }
    });

    //ラジオボタン選択
　　//課税　
    $('#l_z_kubun0').click(function (e) {

        $('input[name=z_kubun]:eq(0)').prop('checked', true);
        $('#d_keihi').css('background-color', 'orange');
        $('#d_syozei').css('background-color', 'orange');
        calc_kingaku($('#d_kingaku'), $('#<%=kounyu_date.ClientID%>').val());
         
        
    });
    //非課税　
    $('#l_z_kubun1').click(function (e) {

        $('input[name=z_kubun]:eq(1)').prop('checked', true);
        $('#d_keihi').css('background-color', 'lightgray');
        $('#d_syozei').css('background-color', 'lightgray');

        calc_kingaku($('#d_kingaku'), $('#<%=kounyu_date.ClientID%>').val());
       
    });

    //その他
    $('#l_z_kubun2').click(function (e) {

        $('input[name=z_kubun]:eq(2)').prop('checked', true);
        $('#d_keihi').css('background-color', 'lightgray');
        $('#d_syozei').css('background-color', 'lightgray');
        calc_kingaku($('#d_kingaku'), $('#<%=kounyu_date.ClientID%>').val());
       
    });

   
    //科目チェンジイベント
    //科目名表示
    $('#d_kan_code').on({
        change: function () {

            $('#err_msg_lbl').val('');
            $('#d_kan_name').css('color', 'black');
            $('#d_uti_code').prop('disabled', false);
            $('#uti_serach').prop('disabled', false);

            var kamoku_cd = document.getElementById("d_kan_code").value;
            var wk_KAMOKU_NAME = document.getElementById("d_kan_name");
            var wk_UCHIWAKE_NAME = document.getElementById("d_uti_name");
            wk_UCHIWAKE_NAME.text='';
            $('#d_uti_cd').val('');
            if (kamoku_cd.length > 0) {
                //科目名表示用関数
                //科目コードにより税区分の初期値をセット
                $('#d_kan_name').func_kamoku_nm_zei(kamoku_cd, wk_KAMOKU_NAME);

                $('#d_uti_name').func_utiwake_chk(kamoku_cd, $('#d_uti_name'));

            } else {
                $('#d_kan_name').text('');
            }
            //金額編集、消費税計算
           calc_kingaku($('#d_kingaku'), $('#<%=kounyu_date.ClientID%>').val());
         
        }
    });
    //科目選択画面オープン
    $('#kan_serach').click(function (e) {
      
        $('#err_msg_lbl').val('');
        $('table#kamoku *').remove();
        $.ajax({
            type: "POST",
            url: "Form_SYKINDN_New.aspx/GetJOSNData_Kamoku",
            contentType: "application/json;charset=utf-8",
            data: {},
            dataType: "json",
            success: function (data) {

                if (data.d.length > 0) {
                    $('#kamoku').append("<tr style='background-color:#6699FF'><th>科目コード</th><th>科目名</th><th>税区分</th></tr>");

                    for (var i = 0; i < data.d.length; i++) {
                        $('#kamoku').append("<tr onClick='mClickTR_kamoku(this)'><td>" + data.d[i].KAMOKU_CD + "</td> <td>" + data.d[i].KAMOKU_NM + "</td><td>" + data.d[i].TAX_CD + "</td></tr>");
                       // $('#kamoku').append("<tr><td>" + data.d[i].KAMOKU_CD + "</td> <td>" + data.d[i].KAMOKU_NM + "</td><td>" + data.d[i].TAX_CD + "</td></tr>");

                    }
                    di_kamoku.dialog('open');

                } else {
                    $('#msg').text('データは０件でした');
                    di_msg.dialog('open');
                }
       
            },
            error: function (result, errorThrown) {
                $('#msg').text('異常終了！！' + errorThrown);
                di_msg.dialog('open');
                return false;

                //alert("Error login");

            }

        });
     


    });

    //内訳チェンジイベント
    //内訳名表示
    $('#d_uti_code').on({
        change: function () {

            $('#err_msg_lbl').val('');
            $('#d_uti_name').css('color', 'black');

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

        $('#err_msg_lbl').val('');
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
                        $('#utiwake').append("<tr style='background-color:#6699FF'><th>内訳コード</th><th>内訳名</th></tr>");

                        for (var i = 0; i < data.d.length; i++) {
                            $('#utiwake').append("<tr onClick='mClickTR_utiwake(this)'><td>" + data.d[i].UCHI_CD + "</td> <td>" + data.d[i].UCHI_NM + "</td></tr>");

                        }
                        di_utiwake.dialog('open');

                    } else {

                        $('#msg').text('データは０件でした');
                        di_msg.dialog('open');

                    }

                },
                error: function (result, errorThrown) {
                    $('#msg').text('異常終了！！' + errorThrown);
                    di_msg.dialog('open');

                    //alert("Error login");

                }

            });
        }
    });

    $('input[readonly="readonly"]').on('focus', function () {
       
        $(this).blur();
        return false;
    });

    
    //金額の編集と消費税計算
    $('#d_kingaku').on({

        change: function () {
             
            //金額編集、消費税計算
            calc_kingaku($(this), $('#<%=kounyu_date.ClientID%>').val());

        }
    });

    //
    //行クリック選択ラジオンボタンの解除とチェック
    //
    $("#MainContent_GridView1 td").click(function () {
         $('#<%=button_kousin.ClientID%>').prop('disabled','true');
    
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
 

    //明細修正
    //行ダブルクリックで編集画面
    $("#MainContent_GridView1 td").dblclick(function () {
         $('#<%=button_kousin.ClientID%>').prop('disabled','true');
    
        if ($('#err_msg_lbl').val().length > 0) {

            $('#msg').text('エラーが存在します');
            di_msg.dialog('open');

            return false;

        }



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
    

        var col = this.cellIndex;
        var bumon_cd = $(this).closest("tr").find("td").eq(1).text();

        //空クックを防止
        if (bumon_cd === ' ') {
            $('#msg').text('データが存在しません');
            di_msg.dialog('open');
            return false;
        }
    
        var bumon_nm = $(this).closest("tr").find("td").eq(2).text();
        var shiharai = $(this).closest("tr").find("td").eq(3).text();
        var kamoku_cd = $(this).closest("tr").find("td").eq(4).text();
        var kamoku_nm = $(this).closest("tr").find("td").eq(5).text();
        var utiwake_cd = $(this).closest("tr").find("td").eq(6).text();
        var utiwake_nm = $(this).closest("tr").find("td").eq(7).text();
        var tekiyo = $(this).closest("tr").find("td").eq(8).text();
        var zkubun = $(this).closest("tr").find("td").eq(9).text();
        var zname = $(this).closest("tr").find("td").eq(10).text();
        var keihi = $(this).closest("tr").find("td").eq(11).text();
        var zei = $(this).closest("tr").find("td").eq(12).text();
        var kin = $(this).closest("tr").find("td").eq(13).text();


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

        var wk_ZEIKUBUN = document.getElementsByName("z_kubun");

        var wk_KINGAKU = document.getElementById("d_kingaku");
        var wk_KEIHI = document.getElementById("d_keihi");
        var wk_ZEI = document.getElementById("d_syozei");
        var wk_GOUKEI = document.getElementById("d_gokei");

        //科目ｺｰﾄﾞにて内訳ｺｰﾄﾞの存在チェック
        $('#d_uti_code').prop('disabled', false);
        $('#uti_serach').prop('disabled', false);

        $('#d_uti_name').func_utiwake_chk(kamoku_cd, wk_UCHIWAKE_NAME);
     

        //部門名表示用関数
        //$('GridView1').func_bumon_nm(bumon_cd, wk_BUMON_NAME);
        $('#d_bumon_name').func_bumon_nm(bumon_cd, wk_BUMON_NAME);

        wk_BUMON_CD.value = bumon_cd;
        wk_SHIHARAI.value = shiharai;
        wk_KAMOKU_CD.value = kamoku_cd;
        wk_KAMOKU_NAME.innerHTML = kamoku_nm;
        wk_UCHIWAKE_CD.value = utiwake_cd;
        wk_UCHIWAKE_NAME.innerHTML=  utiwake_nm;
        wk_TEKIYO.value = tekiyo;

       ////********************************************************
        if (zkubun === '1' || zkubun === '2') {
            //課税にチェック
            $('input[name=z_kubun]:eq(0)').prop('checked', true);
            $('#d_keihi').css('background-color', 'orange');
            $('#d_syozei').css('background-color', 'orange');

        }
        if (zkubun === '3') {
            //非課税にチェック
            $('input[name=z_kubun]:eq(1)').prop('checked', true);
            $('#d_keihi').css('background-color', 'lightgray');
            $('#d_syozei').css('background-color', 'lightgray');

        }
        if (zkubun === '4') {
            //その他にチェック
            $('input[name=z_kubun]:eq(2)').prop('checked', true);
            $('#d_keihi').css('background-color', 'lightgray');
            $('#d_syozei').css('background-color', 'lightgray');

        }


        wk_KINGAKU.value = kin;

        wk_KEIHI.value = keihi;
        wk_ZEI.value = zei;
        wk_GOUKEI.value = kin;

    
    });
    // 行削除ボタンのイベントハンドラ
    $('#<%=gyo_del.ClientID%>').click(function () {
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

    //ユーザー選択するイベント
function mClickTR_user(obj) {
        $('#err_msg_lbl').val('');
        $('#<%=gyo_new.ClientID%>').prop('disabled', false);

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
    $('#err_msg_lbl').val('');
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
    $('#err_msg_lbl').val('');
    $('#d_uti_code').prop('disabled', false);
    $('#uti_serach').prop('disabled', false);
    $('#d_kan_name').css('color', 'black'); 

    var cd = document.getElementById('d_kan_code');
    var nm = document.getElementById('d_kan_name');
    var tax = document.getElementById('d_tax');
    var wk_UTIWAKE_NAME = document.getElementById("d_uti_name");

    cd.value = obj.cells[0].innerHTML;
    nm.innerHTML = obj.cells[1].innerHTML;
    tax.value = obj.cells[2].innerHTML;
    $('#d_uti_code').func_utiwake_chk(cd.value, wk_UTIWAKE_NAME);

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
    //消費税再計算
    var pdate = $('#<%=kounyu_date.ClientID%>').val();
    var nm_zei = document.getElementById('d_zeiritu');
    var kin_val_wk = document.getElementById('d_kingaku').value;
    var kin_val = kin_val_wk.replace(/,/g, "");

    if (kin_val > "") {
        alert('uuuu');
        if ($('input[name="z_kubun"]:eq(0)').is(':checked')) {
              $('#d_zeiritu').func_zeiritu(pdate, nm_zei, kin_val);
        }
   }

}

//内訳選択するイベント
function mClickTR_utiwake(obj) {

    $('#err_msg_lbl').val('');
    $('#d_uti_name').css('color', 'black');
    var cd = document.getElementById('d_uti_code');
    var nm = document.getElementById('d_uti_name');
    console.log(cd);
    console.log(nm);
    cd.value = obj.cells[0].innerHTML;
    nm.innerHTML = obj.cells[1].innerHTML;
   
}

</script>
<div id="Container">

 <div class="hidden" id ="hidden5">
 <div id="aaa">テストA</div>
 </div>
 <div class="hidden" id ="hidden7">
 <input type="hidden" id="wk_bumoncd" name="wk_bumoncd" />
 <input type="hidden" id="wk_bumonnm" name="wk_bumonnm" />
 <input type="hidden" id="wk_shiharai" name="wk_shiharai" />
 <input type="hidden" id="wk_kamokucd" name="wk_kamokucd" />
 <input type="hidden" id="wk_kamokunm" name="wk_kamokunm" />
 <input type="hidden" id="wk_uchiwakecd" name="wk_uchiwakecd" />
 <input type="hidden" id="wk_uchiwakenm" name="wk_uchiwakenm" />
 <input type="hidden" id="wk_tekiyo" name="wk_tekiyo" />
 <input type="hidden" id="wk_zkubun" name="wk_zkubun" />
 <input type="hidden" id="wk_zkubunnm" name="wk_zkubunnm" />
 </div>
 
     
<div class="hidden" id ="gokueikingaku">
<asp:TextBox type="hidden" ID="h_maxgyo" runat="server"  ></asp:TextBox>
<asp:TextBox type="hidden" ID="keihi_goukei" runat="server"  ></asp:TextBox>
<asp:TextBox type="hidden" ID="zei_goukei" runat="server"  ></asp:TextBox>
<asp:TextBox type="hidden" ID="kin_goukei" runat="server"  ></asp:TextBox>
 </div>

 <div class="hidden" id="hidden6">
 </div>
      <div id="title" class="index1">
      出金伝票入力 </div>
<div>
<asp:TextBox type="hidden" ID="auth_user_id" runat="server"  ></asp:TextBox>
<asp:TextBox type="hidden" ID="auth_user_name" runat="server"  ></asp:TextBox>
<asp:TextBox type="hidden" ID="jibumoncd" runat="server"  ></asp:TextBox>
<asp:TextBox type="hidden" ID="manageno" runat="server"  ></asp:TextBox>
<asp:TextBox type="hidden" class="syubetu" ID="syubetu" name="syubetu" runat="server" BorderStyle="None" ></asp:TextBox>

</div>
<div class="header0">
<asp:label runat="server" class="denban_lbl">伝票番号</asp:label><asp:TextBox cssclass="denban input-sm" ID="denban1" name="denban1" runat="server" tabindex="-1" readonly="true"  ></asp:TextBox>
<asp:TextBox type="hidden" class="h_denban" ID="h_denban" name="h_denban" runat="server"  ></asp:TextBox>
<asp:label runat="server" class="zandaka_lbl">残高</asp:label><asp:TextBox cssclass="input-sm zandaka" ID="zandaka" tabindex="-1" runat="server" readonly="true" ></asp:TextBox>
    <br />
   <asp:Panel class="panel" runat="server" BorderStyle="Solid"  BackColor ="WhiteSmoke">
   <table class="header_group1">
    <tr>
     <td>購入日<asp:TextBox cssclass="div_kounyu_date　datepicker input-sm" id="kounyu_date" runat="server" style="margin-left:5px" ></asp:TextBox></td>
     <td></td> 
    </tr>
      <tr>
         <td>氏名<input type="text" name="d_user_cd" id="d_user_cd" class="input-sm"/><input id="user_serach" type="button" class="btn btn-default" value="..." style="font-size :small"></td>
         <td><input type="text" name="d_user_name" id="d_user_name"  tabindex="-1" class="input-sm" style="border-style: none;width:200px" readonly="readonly"/></td>
       </tr>
</table>
        </asp:Panel>
    </div>
<div class="meisai" >
<asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical"  HorizontalAlign="Center" Width="100%"
            Height="280px" >

      <asp:GridView runat="server" ID="GridView1"  HeaderStyle-BackColor  ="#0857E0" Width="100%" >
        <Columns>
            <asp:TemplateField HeaderText="選択" HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate >
                    <asp:RadioButton ID="RadioButton1" runat="server"  />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle cssclass="fixdetail" BackColor="lightgray" Height="15px" ></HeaderStyle>
               <PagerStyle Width="100px" />
     </asp:GridView>
</asp:Panel>
<div>
<asp:Panel ID="Panel2" runat="server" HorizontalAlign="Center" Width="100%">
<div id="goukei_ran" class="goukei_ran">
    <table id="gokei_table" border="1">
        <tr><td style="background-color:#6699FF;width:100px">合計</td>
            <td id="goukeiran_keihi" style="width:150px;text-align:right;"><asp:TextBox borderstyle="None" style="text-align:right;" ID="keihi_goukeiran" runat="server" readonly="true" tabindex="-1" ></asp:TextBox></td>
            <td id="goukeiran_zei" style="width:150px;text-align:right;"><asp:TextBox borderstyle="None" style="text-align:right;" ID="zei_goukeiran"  runat="server" readonly="true"  tabindex="-1"></asp:TextBox></td>
            <td id="goukeiran_kin" style="width:150px;text-align:right;"><asp:TextBox borderstyle="None" style="text-align:right;" ID="kin_goukeiran"  runat="server" readonly="true"  tabindex="-1"></asp:TextBox></td>
        </tr>
    </table>
</div>
</asp:Panel>
    </div>
<div class="gyobtn">
<asp:button id="gyo_new" runat="server" cssclass="btn btn-default" text="行追加" style="font-size:small"/>
<asp:button id="gyo_del" runat="server" cssclass="btn btn-default" text="行削除" style="font-size:small"/>
<p></p>
<input type="button" class="btn btn-default" id="button_kakunin" value="更新" style="font-size:larger"/>
<asp:button cssclass="hidden btn btn-default" runat="server"  id="button_kousin" text="更新" style="font-size:larger"/>
<asp:button cssclass="btn btn-default" runat="server"  id="button_delete" text="削除" style="font-size:larger"/>
<asp:button cssclass="btn btnClose btn-default" id="btnClose"  runat="server" style="font-size:larger" text="閉じる" />
<asp:label  id="msgbox"  runat="server" style="font-size:larger;margin-left:150px" ></asp:label>
<asp:button cssclass="hidden" runat="server"  id="move_itiran" text="" />
<asp:button cssclass="hidden" runat="server"  id="gyo_insert" text="" />
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
<div id="dialog_syukin" class="dialog_syukin" title="出金伝票">
 <input type="hidden" id="d_gyo" />
<div > 
<label> 部門</label><input type="text"id="d_bumon_id" class="input-sm" style="font-size:small;width:100px;margin-left:5px;margin-top:10px"/><input id="bumon_serach" type="button" value="..." class="btn btn-default">
 <label id="d_bumon_name" ></label>
</div>
<div>
<label >支払</label><input type="text" class="input-sm" id="d_shiharai" style="font-size:small;margin-left:5px;margin-top:10px;width:500px" />
</div>
    <div>
<label >科目</label><input type="text" id="d_kan_code" class="input-sm" style="font-size:small;width:100px;margin-left:5px;margin-top:10px"/><input id="kan_serach" type="button" value="..." class="btn btn-default">
<label id="d_kan_name"></label>
</div>
    <div>
<label>内訳</label><input type="text" id="d_uti_code" class="input-sm" style="font-size:small;width:100px;margin-left:5px;margin-top:10px"/><input id="uti_serach" type="button" value="..." class="btn btn-default">
<label id="d_uti_name"></label>
    </div>
<div>
<label>摘要</label><input type="text" id="d_tekiyo" class="input-sm" style="font-size:small;margin-left:5px;margin-top:10px;width:500px" />
</div>
<div >
<input type="radio" id="d_z_kubun0" name="z_kubun" value="0" style="margin-top:10px;width:10px" /><label id="l_z_kubun0" style="margin-top:10px;width:100px;color:darkgreen ">課税</label>
<input type="radio" id="d_z_kubun1" name="z_kubun" value="1" style="margin-top:10px;width:10px" /><label id="l_z_kubun1" style="margin-top:10px;width:100px;color:darkgreen">非課税</label>
<input type="radio" id="d_z_kubun2" name="z_kubun" value="2" style="margin-top:10px;width:10px" /><label id="l_z_kubun2" style="margin-top:10px;width:100px;color:darkgreen">その他</label>
</div>
<div>
<label>金額</label><input type="text" id="d_kingaku" class="input-sm" style="font-size:small;margin-left:5px;margin-top:10px;text-align:right;width:150px" />
</div>
<input type="hidden" id="d_tax"/>
<input type="hidden" id="d_zeiritu"/>
    <asp:panel runat="server" BorderStyle="Solid" style="margin-top:10px;" Width="300px" BorderWidth="1" >
        <table style="margin:5px">
            <tr>
                <td><label style="color:darkgreen">経費</label></td><td><input type="text" id="d_keihi" class="input-sm" tabindex="-1" style="margin-left:5px;text-align:right;width:150px" readonly="readonly"/></td>
            </tr>
            <tr>
                <td><label style="color:darkgreen">消費税</label></td><td><input type="text" id="d_syozei" class="input-sm" tabindex="-1" style="margin-left:5px;text-align:right;width:150px" readonly="readonly"/></td>
            </tr>
            <tr>
                <td><label style="color:darkgreen">合計額</label> </td><td><input type="text" id="d_gokei" class="input-sm" tabindex="-1" style="margin-left:5px;text-align:right;width:150px" readonly="readonly"/></td>
            </tr>
        </table>
        </asp:panel>
<p></p>
<div id="msg_red" style="display:none; font-size:smaller ;color:red;"></div>
<input type="text" class="hidden" id="err_msg_lbl" /> 

</div> 
<!--新規登録画面の終了-->

<!--各種マスタの選択画面-->
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
<div id="dialog_denpyo_delete" title="メッセージ">
</div>

<!--行削除メッセージ画面-->
<div id="dialog_delete" title="メッセージ">
行削除します　よろしいですか?
</div>

<div id="dialog_YesNo" title="メッセージ">
<label id="msg2" style="font-size:small;"></label> 
<input type="text" class="hidden" id="err_chk_flg" /> 
</div>

</div>
</asp:Content>